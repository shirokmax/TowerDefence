using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceShooter
{
    public enum Team
    {
        Neutral,
        Player,
        Enemy
    }

    /// <summary>
    /// ������������ ������ �� �����. ��, ��� ����� ����� ���������.
    /// </summary>
    public class Destructible : Entity
    {
        #region Properties
        /// <summary>
        /// ������ ���������� �����������.
        /// </summary>
        [SerializeField] protected bool m_Indestructible;
        public bool IsIndestructible => m_Indestructible;

        /// <summary>
        /// ��������� ���-�� ����������.
        /// </summary>
        [SerializeField] protected int m_MaxHitPoints;
        public int MaxHitPoints => m_MaxHitPoints;

        /// <summary>
        /// ������� ���������.
        /// </summary>
        private int m_CurrentHitPoints;
        public int CurrentHitPoints => m_CurrentHitPoints;

        /// <summary>
        /// �������, ���������� ��� "������" destructible.
        /// </summary>
        [Space]
        [SerializeField] protected UnityEvent m_EventOnDeath;
        public UnityEvent EventOnDeath => m_EventOnDeath;

        private UnityEvent m_EventChangeHitPoints = new UnityEvent();
        public UnityEvent EventChangeHitPoints => m_EventChangeHitPoints;

        private UnityEvent m_EventOnDamageTaken = new UnityEvent();
        public UnityEvent EventOnDamageTaken => m_EventOnDamageTaken;

        private UnityEvent<Destructible, int> m_EventOnDamageTakenBy = new UnityEvent<Destructible, int>();
        public UnityEvent<Destructible, int> EventOnDamageTakenBy => m_EventOnDamageTakenBy;

        private static HashSet<Destructible> m_AllDestructibles;
        public static IReadOnlyCollection<Destructible> AllDestructibles => m_AllDestructibles;

        [SerializeField] private Team m_TeamId;
        public Team Team => m_TeamId;

        [SerializeField][Range(0f, 1f)] private float m_FriendlyFirePercentage;
        public float FriendlyFirePercentage => m_FriendlyFirePercentage;

        #endregion

        #region Unity Events
        protected virtual void Start()
        {
            m_CurrentHitPoints = m_MaxHitPoints;
            m_EventChangeHitPoints.Invoke();
        }

        #endregion

        #region Public API
        /// <summary>
        /// ���������� ����� � �������.
        /// </summary>
        public bool ApplyDamage(int damage)
        {
            return ApplyDamage(null, damage);
        }

        /// <summary>
        /// ���������� ����� � ������� � ����������� "����� �� �����".
        /// </summary>
        /// <param name="damage">����, ��������� �������.</param>
        /// <param name="teamId">Id ������� �������, ���������� ����.</param>
        /// <param name="friendlyFirePercentage">������� "����� �� �����" �������, ���������� ����.</param>
        /// <returns>���������� ��������� ��������.</returns>
        public bool ApplyDamage(Destructible fromDest, int damage)
        {
            if (IsIndestructible) return false;

            if (damage == 0) return false;

            if (fromDest != null)
            {
                if (fromDest.Team == m_TeamId && fromDest.FriendlyFirePercentage == 0) return false;

                if (fromDest.Team == m_TeamId && fromDest.FriendlyFirePercentage > 0.0f)
                {
                    int dmg = (int)(damage * fromDest.FriendlyFirePercentage);

                    m_CurrentHitPoints -= dmg;
                    m_EventOnDamageTaken.Invoke();
                    m_EventOnDamageTakenBy.Invoke(fromDest, dmg);
                }
                else
                {
                    m_CurrentHitPoints -= damage;
                    m_EventOnDamageTaken.Invoke();
                    m_EventOnDamageTakenBy.Invoke(fromDest, damage);
                }
            }
            else
            {
                m_CurrentHitPoints -= damage;
                m_EventOnDamageTaken.Invoke();
                m_EventOnDamageTakenBy.Invoke(fromDest, damage);
            }

            m_EventChangeHitPoints.Invoke();

            if (m_CurrentHitPoints <= 0)
            {
                m_CurrentHitPoints = 0;

                OnDeath();
            }

            return true;
        }

        /// <summary>
        /// ����������� �������� � �������. �������� ������� �� ����� ����� ������ �������������.
        /// </summary>
        /// <param name="healAmount">���-�� ������������� ��������.</param>
        public bool Heal(int healAmount)
        {
            if (m_CurrentHitPoints < m_MaxHitPoints)
            {
                if (m_CurrentHitPoints + healAmount > m_MaxHitPoints)
                    m_CurrentHitPoints = m_MaxHitPoints;
                else
                    m_CurrentHitPoints += healAmount;

                m_EventChangeHitPoints.Invoke();

                return true;
            }

            return false;
        }

        public void SetTeamId(Team id)
        {
            m_TeamId = id;
        }

        #endregion

        #region Protected API
        /// <summary>
        /// ���������������� ������� ����������� �������, ����� ��������� ���� ����.
        /// </summary>
        protected virtual void OnDeath()
        {
            Destroy(gameObject);

            m_EventOnDeath?.Invoke();
        }

        protected virtual void OnEnable()
        {
            if (m_AllDestructibles == null)
                m_AllDestructibles = new HashSet<Destructible>();

            m_AllDestructibles.Add(this);
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();

            m_AllDestructibles.Remove(this);
        }

        #endregion
    }
}
