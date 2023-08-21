using UnityEngine;

namespace SpaceShooter
{
    [CreateAssetMenu]
    public sealed class TurretProperties : ScriptableObject
    {
        [SerializeField] private Projectile m_ProjectilePrefab;
        public Projectile ProjectilePrefab => m_ProjectilePrefab;

        [SerializeField] private float m_FireRate = 1f;
        public float FireRate => m_FireRate;
    }
}
