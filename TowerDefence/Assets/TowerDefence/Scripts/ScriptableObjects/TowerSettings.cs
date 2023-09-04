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

        [SerializeField] private Color m_TowerSpriteColor = Color.white;
        public Color TowerSpriteColor => m_TowerSpriteColor;

        [SerializeField] private Vector2 m_TowerSpriteScale = Vector2.one;
        public Vector2 TowerSpriteScale => m_TowerSpriteScale;

        [Space]
        [SerializeField] private Vector2 m_GroundSpriteScale = new Vector2(1.2f, 1.2f);
        public Vector2 GroundSpriteScale => m_GroundSpriteScale;

        [SerializeField] private Vector2 m_GroundPosition = Vector2.zero;
        public Vector2 GroundPosition => m_GroundPosition;

        [Space]
        [SerializeField] private Vector2 m_ShadowSpriteScale = Vector2.one;
        public Vector2 ShadowSpriteScale => m_ShadowSpriteScale;

        [SerializeField] private Vector2 m_ShadowPosition = Vector2.zero;
        public Vector2 ShadowPosition => m_ShadowPosition;

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

        [Space]
        [SerializeField] private bool m_StartTower;
        public bool StartTower => m_StartTower;

        [SerializeField] private TowerSettings[] m_UpgradesTo;
        public TowerSettings[] UpgradesTo => m_UpgradesTo;

        [Space]
        [SerializeField] private UpgradeAsset m_AttackRangeUpgrade;
        public UpgradeAsset AttackRangeUpgrade => m_AttackRangeUpgrade;

        [SerializeField] private UpgradeAsset m_DamageUpgrade;
        public UpgradeAsset DamageUpgrade => m_DamageUpgrade;

        [SerializeField] private UpgradeAsset m_AttackSpeedUpgrade;
        public UpgradeAsset AttackSpeedUpgrade => m_AttackSpeedUpgrade;

        [Space]
        [SerializeField] private UpgradeAsset m_UnitsHitPointsUpgrade;
        public UpgradeAsset UnitsHitPointsUpgrade => m_UnitsHitPointsUpgrade;

        [SerializeField] private UpgradeAsset m_UnitsDamageUpgrade;
        public UpgradeAsset UnitsDamageUpgrade => m_UnitsDamageUpgrade;

        [SerializeField] private UpgradeAsset m_UnitsRespawnTimeUpgrade;
        public UpgradeAsset UnitsRespawnTimeUpgrade => m_UnitsRespawnTimeUpgrade;
    }
}
