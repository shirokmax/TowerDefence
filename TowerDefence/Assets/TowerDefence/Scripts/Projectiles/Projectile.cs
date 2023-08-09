using TowerDefence;
using UnityEngine;

namespace SpaceShooter
{
    public class Projectile : Entity
    {
        [SerializeField] protected float m_Speed;
        public float Speed => m_Speed;

        [SerializeField] protected float m_Lifetime;
        [SerializeField] protected int m_Damage;
        public int Damage => m_Damage;
        [SerializeField] protected ImpactEffect m_LaunchSFXPrefab;
        [SerializeField] protected ImpactEffect m_HitEffectPrefab;
        [SerializeField] protected ImpactEffect m_HitSFXPrefab;

        protected Destructible m_ParentDest;
        protected Destructible m_TargetDest;

        protected bool m_IsParentPlayer;

        protected float m_Timer;

        private void Start()
        {
            if (m_LaunchSFXPrefab != null)
                Instantiate(m_LaunchSFXPrefab, transform.position, Quaternion.identity);
        }

        private void Update()
        {
            ProjectileMovement();
        }

        protected virtual void ProjectileMovement()
        {
            float stepLength = m_Speed * Time.deltaTime;
            Vector2 step = transform.up * stepLength;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, stepLength);

            if (hit)
            {
                if (hit.collider.transform.root.TryGetComponent(out Unit unit) && unit == m_TargetDest)
                {
                    unit.TakeDamage(m_Damage);
                    OnProjectileHit(hit.point);
                }
            }

            m_Timer += Time.deltaTime;

            if (m_Timer >= m_Lifetime)
                Destroy(gameObject);

            transform.position += new Vector3(step.x, step.y, 0);
        }

        protected void OnProjectileHit(Vector2 pos)
        {
            Destroy(gameObject);

            if (m_HitEffectPrefab != null)
                Instantiate(m_HitEffectPrefab, pos, Quaternion.identity);

            if (m_HitSFXPrefab != null)
                Instantiate(m_HitSFXPrefab, pos, Quaternion.identity);
        }

        public void SetParentShooter(Destructible dest)
        {
            m_ParentDest = dest;
        }

        public void SetTarget(Destructible target)
        {
            m_TargetDest = target;
        }

        public void AddDamage(int damage)
        {
            m_Damage += damage;
        }
    }
}