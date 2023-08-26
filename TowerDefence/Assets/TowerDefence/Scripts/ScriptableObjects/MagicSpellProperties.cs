using UnityEngine;

namespace TowerDefence
{
    public enum MagicSpellType
    {
        Fire,
        Slow
    }

    [CreateAssetMenu]
    public sealed class MagicSpellProperties : ScriptableObject
    {
        [SerializeField] private string m_SpellName;
        public string SpellName => m_SpellName;

        [SerializeField] private Sprite m_DefaultSpellIconSprite;
        public Sprite DefaultSpellIconSprite => m_DefaultSpellIconSprite;

        [SerializeField] private Sprite m_ActiveSpellIconSprite;
        public Sprite ActiveSpellIconSprite => m_ActiveSpellIconSprite;

        [Space]
        [SerializeField] private MagicSpellType m_SpellType;
        public MagicSpellType SpellType => m_SpellType;

        [SerializeField] private int m_ManaCost;
        public int ManaCost => m_ManaCost;

        [SerializeField] private float m_Duration;
        public float Duration => m_Duration;

        [SerializeField] private int m_DamagePerSecond = 200;
        public int DamagePerSecond => m_DamagePerSecond;

        [SerializeField] private int m_DamageTicksPerSecond = 4;
        public int DamageTicksPerSecond => m_DamageTicksPerSecond;

        [SerializeField] private float m_Radius = 0.8f;
        public float Radius => m_Radius;

        [SerializeField] private float m_SlowRate = 0.5f;
        public float SlowRate => m_SlowRate;

        [Space]
        [SerializeField] private UpgradeAsset m_ManaCostUpgrade;
        public UpgradeAsset ManaCostUpgrade => m_ManaCostUpgrade;

        [SerializeField] private UpgradeAsset m_DurationUpgrade;
        public UpgradeAsset DurationUpgrade => m_DurationUpgrade;

        [SerializeField] private UpgradeAsset m_DamagePerSecondUpgrade;
        public UpgradeAsset DamagePerSecondUpgrade => m_DamagePerSecondUpgrade;

        [SerializeField] private UpgradeAsset m_SlowRateUpgrade;
        public UpgradeAsset SlowRateUpgrade => m_SlowRateUpgrade;
    }
}
