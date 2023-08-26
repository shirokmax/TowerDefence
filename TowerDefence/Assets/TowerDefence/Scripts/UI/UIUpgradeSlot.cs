using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public enum SlotType
    {
        Upgrade,
        TowerStatsInfo,
        MagicSpellStatsInfo
    }

    public class UIUpgradeSlot : MonoBehaviour
    {
        [SerializeField] private SlotType m_SlotType;
        public SlotType SlotType => m_SlotType;

        [SerializeField] private UpgradeAsset m_UpgradeAsset;
        public UpgradeAsset UpgradeAsset => m_UpgradeAsset;

        [Space]
        [SerializeField] private UpgradeAsset m_RequireUpgrade;

        [Space]
        [SerializeField] private TowerSettings[] m_TowerSettings;
        public TowerSettings[] TowerSettings => m_TowerSettings;

        [SerializeField] private MagicSpellProperties m_MagicSpellProperties;
        public MagicSpellProperties MagicSpellProperties => m_MagicSpellProperties;

        [Space]
        [SerializeField] private Image m_IconImage;
        public Image IconImage => m_IconImage;

        [SerializeField] private Button m_SelectButton;
        [SerializeField] private Image m_UpgradeLevelWindowImage;
        [SerializeField] private Text m_LevelNumText;

        [SerializeField] private Image m_ActiveIconImage;
        public Image ActiveIconImage => m_ActiveIconImage;

        private void Awake()
        {
            m_ActiveIconImage.gameObject.SetActive(false);
        }

        public void Initialize()
        {
            switch (m_SlotType)
            {
                case SlotType.Upgrade:
                    m_IconImage.sprite = m_UpgradeAsset.IconSprite;
                    break;
                case SlotType.TowerStatsInfo:
                    m_IconImage.sprite = m_TowerSettings[0].TowerGUISprite;
                    break;
                case SlotType.MagicSpellStatsInfo:
                    m_IconImage.sprite = m_MagicSpellProperties.DefaultSpellIconSprite;
                    break;
                default:
                    break;
            }

            UpdateSlot();
        }

        public void UpdateSlot()
        {
            if (m_SlotType == SlotType.Upgrade)
            {
                if (m_RequireUpgrade != null && Upgrades.GetUpgradeLevel(m_RequireUpgrade) == 0)
                {
                    m_SelectButton.interactable = false;
                    m_UpgradeLevelWindowImage.color = m_SelectButton.colors.disabledColor;
                }
                else
                {
                    m_SelectButton.interactable = true;
                    m_UpgradeLevelWindowImage.color = Color.white;
                }

                int savedLevel = Upgrades.GetUpgradeLevel(m_UpgradeAsset);

                if (savedLevel == m_UpgradeAsset.CostsAndValues.Length)
                    m_LevelNumText.text = "max";
                else
                    m_LevelNumText.text = $"lvl {savedLevel + 1}";
            }
        }

        public void SelectSlot()
        {
            UIUpgradeShop.Instance.SelectUpgradeSlot(this);
        }
    }
}
