using TowerDefence;
using UnityEngine;

namespace SpaceShooter
{
    public enum DamageType
    {
        Physical,
        Magic
    }

    public class Projectile : Entity
    {
        [Space]
        [SerializeField] private DamageType m_DamageType;
        public DamageType DamageType => m_DamageType;

        [SerializeField] private int m_Damage;
        public int Damage => m_Damage;

        [SerializeField] private float m_Speed;
        public float Speed => m_Speed;

        /// <summary>
        /// Границы касания ракеты других коллайдеров.
        /// </summary>
        [Space]
        [SerializeField] private Vector2 m_HitBounds;

        [Space]
        [SerializeField] private bool m_SplashDamage;

        /// <summary>
        /// Радиус сплеш урона.
        /// </summary>
        [SerializeField] private float m_SplashDamageRadius;

        /// <summary>
        /// Скорость плавного поворота к цели.
        /// </summary>
        [Space]
        [SerializeField] private bool m_HomingRotateInterpolation;
        [SerializeField] private float m_HomingRotateInterpolationValue;

        [Space]
        [SerializeField] private ImpactEffect m_LaunchSFXPrefab;
        [SerializeField] private ImpactEffect m_HitSFXPrefab;
        [SerializeField] private ImpactEffect m_HitEffectPrefab;

        /// <summary>
        /// Множитель размера эффекта взрыва ракеты в зависимости от радиуса урона взрыва.
        /// </summary>
        [SerializeField] private float m_HitEffectScaleMult = 1f;

        private Destructible m_ParentDest;
        private Destructible m_TargetDest;

        private Vector2 m_HomingTargetPosition;

        private const float TARGET_POSITION_THRESHOLD = 0.2f;

        private void Start()
        {
            if (m_LaunchSFXPrefab != null)
                Instantiate(m_LaunchSFXPrefab, transform.position, Quaternion.identity);
        }

        private void Update()
        {
            if (m_TargetDest != null)
            {
                m_HomingTargetPosition = m_TargetDest.transform.position;
            }
            else if ((m_HomingTargetPosition - (Vector2)transform.position).sqrMagnitude <= TARGET_POSITION_THRESHOLD * TARGET_POSITION_THRESHOLD)
            {
                if (m_SplashDamage == true) 
                    OnHit();

                OnLifeEnd();
            }

            Vector2 dir = m_HomingTargetPosition - (Vector2)transform.position;

            if (m_HomingRotateInterpolation == true)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, dir), m_HomingRotateInterpolationValue * Time.deltaTime);
            else
                transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);

            transform.Translate(Vector3.up * m_Speed * Time.deltaTime);

            Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position, m_HitBounds, transform.eulerAngles.z);

            if (hitColliders.Length != 0)
            {
                foreach (Collider2D hitCollider in hitColliders)
                {
                    if (hitCollider.transform.root.TryGetComponent(out Unit unit) && unit == m_TargetDest)
                    {
                        if (m_SplashDamage == true)
                            OnHit();
                        else
                            unit.TakeDamage(m_Damage, m_DamageType);

                        OnLifeEnd();
                    }
                }
            }
        }

        private void OnHit()
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, m_SplashDamageRadius);

            if (hitColliders.Length != 0)
            {
                foreach (Collider2D hitCollider in hitColliders)
                {
                    if (hitCollider.transform.root.TryGetComponent(out Unit unit) && unit != m_ParentDest)
                        unit.TakeDamage(m_ParentDest, m_Damage, m_DamageType);
                }
            }
        }

        private void OnLifeEnd()
        {
            if (m_HitEffectPrefab != null)
            {
                ImpactEffect hitEffect = Instantiate(m_HitEffectPrefab, transform.position, Quaternion.identity);

                if (m_SplashDamage == true)
                    hitEffect.transform.localScale = Vector3.one * (m_SplashDamageRadius * m_HitEffectScaleMult);
            }

            if (m_HitSFXPrefab != null)
                Instantiate(m_HitSFXPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject);
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

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = new Color(1, 0, 0, 0.1f);
            UnityEditor.Handles.DrawSolidDisc(transform.position, transform.forward, m_SplashDamageRadius);

            Gizmos.color = Color.green;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, m_HitBounds);
        }
#endif
    }
}