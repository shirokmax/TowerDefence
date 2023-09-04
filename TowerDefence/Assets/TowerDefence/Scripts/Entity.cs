using TowerDefence;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceShooter
{
    public enum EntityType
    {
        Tower,
        Unit,
        Projectile,
    }

    /// <summary>
    /// ������� ����� ��� ���� ������������� �������� �� �����.
    /// </summary>
    public abstract class Entity : MonoBehaviour
    {
        [SerializeField] protected string m_Nickname;
        public string Nickname => m_Nickname;

        [SerializeField] private EntityType m_Type;
        public EntityType Type => m_Type;

        private UnityEvent m_EventOndestroy = new UnityEvent();
        public UnityEvent EventOnDestroy => m_EventOndestroy;

        protected virtual void OnDestroy()
        {
            m_EventOndestroy.Invoke();
        }
    }
}