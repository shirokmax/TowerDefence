using UnityEngine;

namespace SpaceShooter
{
    public enum TurretType
    {
        Archer,
        Magic,
        Bomb,
    }

    [CreateAssetMenu]
    public sealed class TurretProperties : ScriptableObject
    {
        [SerializeField] private TurretType m_Type;
        public TurretType Type => m_Type;

        [SerializeField] private Projectile m_ProjectilePrefab;
        public Projectile ProjectilePrefab => m_ProjectilePrefab;

        [SerializeField] private float m_FireRate = 1f;
        public float FireRate => m_FireRate;
    }
}
