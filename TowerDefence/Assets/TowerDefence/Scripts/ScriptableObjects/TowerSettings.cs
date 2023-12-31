using SpaceShooter;
using UnityEngine;

namespace TowerDefence
{
    [CreateAssetMenu]
    public sealed class TowerSettings : ScriptableObject
    {
        [SerializeField] private string m_Nickname = "Tower";
        public string Nickname => m_Nickname;

        [SerializeField] private Sprite m_TowerGUISprite;
        public Sprite TowerGUISprite => m_TowerGUISprite;

        [Space]
        [SerializeField] private Sprite m_TowerSprite;
        public Sprite TowerSprite => m_TowerSprite;

        [SerializeField] private Vector2 m_TowerSpriteScale = Vector2.one;
        public Vector2 TowerSpriteScale => m_TowerSpriteScale;

        [SerializeField] private Vector2 m_ShadowSpriteScale = Vector2.one;
        public Vector2 ShadowSpriteScale => m_ShadowSpriteScale;

        [SerializeField] private Vector2 m_ShadowPosition = Vector2.zero;
        public Vector2 ShadowPosition => m_ShadowPosition;

        [Space]
        [SerializeField] private Vector2 m_TowerPositionOffset = Vector2.zero;
        public Vector2 TowerPositionOffset => m_TowerPositionOffset;

        [Space]
        [SerializeField] private TurretProperties m_TurretProps;
        public TurretProperties TurretProps => m_TurretProps;

        [SerializeField] private Vector2 m_TurretPosition = Vector2.zero;
        public Vector2 TurretPosition => m_TurretPosition;

        [Space]
        [SerializeField] private float m_Radius = 2.5f;
        public float Radius => m_Radius;

        [SerializeField] private int m_GoldCost = 70;
        public int GoldCost => m_GoldCost;

        [Space]
        [SerializeField] private bool m_SpawnUnits;
        public bool SpawnUnits => m_SpawnUnits;

        [SerializeField] private Vector2 m_UnitSpawnerPosition = Vector2.zero;
        public Vector2 UnitSpawnerPosition => m_UnitSpawnerPosition;

        [SerializeField] private float m_UnitSpawnerRadius = 0.13f;
        public float UnitSpawnerRadius => m_UnitSpawnerRadius;

        [Space]
        [SerializeField] private UnitSettings m_UnitSettings;
        public UnitSettings UnitSettings => m_UnitSettings;

        [SerializeField] private int m_UnitsSpawnCount;
        public int UnitsSpawnCount => m_UnitsSpawnCount;

        [SerializeField] private float m_UnitsRespawnTime;
        public float UnitsRespawnTime => m_UnitsRespawnTime;

        [SerializeField] private ImpactEffect[] m_UnitSpawnSFXPrefabs;
        public ImpactEffect[] UnitSpawnSFXPrefabs => m_UnitSpawnSFXPrefabs;
    }
}
