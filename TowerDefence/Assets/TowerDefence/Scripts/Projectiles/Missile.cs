using UnityEngine;

namespace SpaceShooter
{
    public class Missile : Projectile
    {
        /// <summary>
        /// Множитель размера эффекта взрыва ракеты в зависимости от радиуса урона взрыва.
        /// </summary>
        [SerializeField] private float m_HitEffectScaleMult = 1f;

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
        /// Максимальный радиус, в котором ракета может захватить первую попавшуюся цель.
        /// </summary>
        [SerializeField] private float m_HomingRadius;

        /// <summary>
        /// Скорость плавного поворота к цели.
        /// </summary>
        [SerializeField] private bool m_HomingRotateInterpolation;
        [SerializeField] private float m_HomingRotateInterpolationValue;

        private Vector2 m_HomingTargetPosition;

        private const float TARGET_POSITION_THRESHOLD = 0.2f;

        protected override void ProjectileMovement()
        {
            m_Timer += Time.deltaTime;

            if (m_Timer >= m_Lifetime)
                Destroy(gameObject);

            if (m_TargetDest != null)
            {
                m_HomingTargetPosition = m_TargetDest.transform.position;
            }
            else if ((m_HomingTargetPosition - (Vector2)transform.position).sqrMagnitude <= TARGET_POSITION_THRESHOLD * TARGET_POSITION_THRESHOLD)
            {
                OnMissileHit();
                OnMissileLifeEnd();
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
                    if (hitCollider.transform.root.TryGetComponent(out Destructible dest) && dest == m_TargetDest)
                    {
                        if (m_SplashDamage == true)
                            OnMissileHit();
                        else
                            dest.ApplyDamage(m_ParentDest, m_Damage);

                        OnMissileLifeEnd();
                    }
                }
            }
        }

        private void OnMissileHit()
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, m_SplashDamageRadius);

            if (hitColliders.Length != 0)
            {
                foreach (Collider2D hitCollider in hitColliders)
                {
                    if (hitCollider.transform.root.TryGetComponent(out Destructible dest) && dest != m_ParentDest)
                        dest.ApplyDamage(m_ParentDest, m_Damage);
                }
            }
        }

        private void OnMissileLifeEnd()
        {
            if (m_HitEffectPrefab != null)
            {
                ImpactEffect hitEffect = Instantiate(m_HitEffectPrefab, transform.position, Quaternion.identity);
                hitEffect.transform.localScale = Vector3.one * (m_SplashDamageRadius * m_HitEffectScaleMult);
            }

            if (m_HitSFXPrefab != null)
                Instantiate(m_HitSFXPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = new Color(1, 1, 0, 0.05f);
            UnityEditor.Handles.DrawSolidDisc(transform.position, transform.forward, m_HomingRadius);

            UnityEditor.Handles.color = new Color(1, 0, 0, 0.1f);
            UnityEditor.Handles.DrawSolidDisc(transform.position, transform.forward, m_SplashDamageRadius);

            Gizmos.color = Color.green;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, m_HitBounds);
        }
#endif
    }
}