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
    /// Уничтожаемый объект на сцене. То, что может иметь хитпоинты.
    /// </summary>
    public class Destructible : Entity
    {
        #region Properties
        /// <summary>
        /// Объект игнорирует повреждения.
        /// </summary>
        [SerializeField] protected bool m_Indestructible;
        public bool IsIndestructible => m_Indestructible;

        /// <summary>
        /// Стартовое кол-во хитпоинтов.
        /// </summary>
        [SerializeField] protected int m_MaxHitPoints;
        public int MaxHitPoints => m_MaxHitPoints;

        /// <summary>
        /// Текущие хитпоинты.
        /// </summary>
        private int m_CurrentHitPoints;
        public int CurrentHitPoints => m_CurrentHitPoints;

        /// <summary>
        /// Событие, вызываемое при "смерти" destructible.
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
        /// Применение урона к объекту.
        /// </summary>
        public bool ApplyDamage(int damage)
        {
            return ApplyDamage(null, damage);
        }

        /// <summary>
        /// Применение урона к объекту с учитыванием "урона по своим".
        /// </summary>
        /// <param name="damage">Урон, наносимый объекту.</param>
        /// <param name="teamId">Id команды объекта, наносящего урон.</param>
        /// <param name="friendlyFirePercentage">Процент "урона по своим" объекта, наносящего урон.</param>
        /// <returns>Возвращает результат операции.</returns>
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
        /// Прибавление здоровья к объекту. Здоровье объекта не может стать больше максимального.
        /// </summary>
        /// <param name="healAmount">Кол-во прибавляемого здоровья.</param>
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
        /// Переопределяемое событие уничтожения объекта, когда хитпоинты ниже нуля.
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
