using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public enum SlotType
    {
        Upgrade,
        TowerStatsInfo
    }

    public class UIUpgradeSlot : MonoBehaviour
    {
        [SerializeField] private SlotType m_SlotType;
        public SlotType SlotType => m_SlotType;

        [SerializeField] private UpgradeAsset m_UpgradeAsset;
        public UpgradeAsset UpgradeAsset => m_UpgradeAsset;

        [Space]
        [SerializeField] private TowerSettings[] m_TowerSettings;
        public TowerSettings[] TowerSettings => m_TowerSettings;

        [Space]
        [SerializeField] private Image m_IconImage;
        public Image IconImage => m_IconImage;

        [SerializeField] private Button m_SelectButton;
        [SerializeField] private Text m_LevelNumText;

        [SerializeField] private Image m_ActiveIconImage;
        public Image ActiveIconImage => m_ActiveIconImage;

        private void Awake()
        {
            m_ActiveIconImage.gameObject.SetActive(false);
        }

        public void Initialize()
        {
            if (m_SlotType == SlotType.Upgrade)
                m_IconImage.sprite = m_UpgradeAsset.IconSprite;

            if (m_SlotType == SlotType.TowerStatsInfo)
                m_IconImage.sprite = m_TowerSettings[0].TowerGUISprite;

            UpdateSlot();
        }

        public void UpdateSlot()
        {
            if (m_SlotType == SlotType.Upgrade)
            {
                int savedLevel = Upgrades.GetUpgradeLevel(m_UpgradeAsset);
                m_LevelNumText.text = $"lvl {savedLevel + 1}";
            }
        }

        public void SelectSlot()
        {
            UIUpgradeShop.Instance.SelectUpgradeSlot(this);
        }
    }
}
