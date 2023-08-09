using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class UIUpgradeShop : MonoSingleton<UIUpgradeShop>
    {
        [SerializeField] private Text m_StarsText;

        [Space]
        [SerializeField] private Image[] m_SelectedUpgradeCostNumerImages;
        [SerializeField] private Image m_StarImage;
        [SerializeField] private Image m_MaxLevelImage;

        [Space]
        [SerializeField] private Text m_SelectedSlotLvlInfoText;
        [SerializeField] private Image m_SelectedUpgradeIconImage;
        [SerializeField] private Text m_SelectedUpgradeDescriptionText;
        [SerializeField] private Button m_BuyUpgradeButton;
        [SerializeField] private Button m_LeftArrowButton;
        [SerializeField] private Button m_RightArrowButton;
        [SerializeField] private Text m_NotEnoghtStarsText;

        [SerializeField] private int m_Stars;

        private UIUpgradeSlot[] m_UpgradeSlots;
        private UIUpgradeSlot m_SelectedUpgrade;

        private int m_SelectedSlotInfoIndex;

        protected override void Awake()
        {
            base.Awake();

            m_UpgradeSlots = GetComponentsInChildren<UIUpgradeSlot>();
        }

        private void Start()
        {
            UpdateStars();

            foreach (var slot in m_UpgradeSlots)
                slot.Initialize();

            SelectUpgradeSlot(m_UpgradeSlots[0]);
        }

        private void UpdateStars()
        {
            m_Stars = MapCompletion.Instance.TotalStars;
            m_Stars -= Upgrades.TotalStarsSpent();
            m_StarsText.text = m_Stars.ToString();
        }

        public void SelectUpgradeSlot(UIUpgradeSlot upgradeSlot)
        {
            foreach (var slot in m_UpgradeSlots)
                slot.ActiveIconImage.gameObject.SetActive(false);

            upgradeSlot.ActiveIconImage.gameObject.SetActive(true);

            foreach (Image upgradeCost in m_SelectedUpgradeCostNumerImages)
                upgradeCost.gameObject.SetActive(false);

            if (m_SelectedUpgrade != upgradeSlot)
                m_SelectedSlotInfoIndex = 0;

            m_SelectedUpgrade = upgradeSlot;

            if (upgradeSlot.SlotType == SlotType.Upgrade)
            {
                m_SelectedUpgradeIconImage.sprite = upgradeSlot.IconImage.sprite;

                m_BuyUpgradeButton.gameObject.SetActive(true);
                m_SelectedSlotLvlInfoText.gameObject.SetActive(false);
                m_LeftArrowButton.gameObject.SetActive(false);
                m_RightArrowButton.gameObject.SetActive(false);

                int lvl = Upgrades.GetUpgradeLevel(upgradeSlot.UpgradeAsset);

                if (lvl >= upgradeSlot.UpgradeAsset.CostsAndValues.Length)
                {
                    m_SelectedUpgradeDescriptionText.gameObject.SetActive(false);
                    m_StarImage.gameObject.SetActive(false);
                    m_MaxLevelImage.gameObject.SetActive(true);
                    m_BuyUpgradeButton.interactable = false;
                }
                else
                {
                    m_SelectedUpgradeDescriptionText.text = upgradeSlot.UpgradeAsset.Description +
                                                            Upgrades.GetUpgradeValueByLevel(upgradeSlot.UpgradeAsset, Upgrades.GetUpgradeLevel(upgradeSlot.UpgradeAsset) + 1) +
                                                            upgradeSlot.UpgradeAsset.PostValueDescription;

                    m_MaxLevelImage.gameObject.SetActive(false);
                    m_StarImage.gameObject.SetActive(true);

                    int cost = upgradeSlot.UpgradeAsset.CostsAndValues[lvl].Cost;
                    m_SelectedUpgradeCostNumerImages[cost - 1].gameObject.SetActive(true);

                    if (m_Stars >= cost)
                    {
                        m_NotEnoghtStarsText.gameObject.SetActive(false);
                        m_BuyUpgradeButton.GetComponent<Image>().color = Color.white;
                        m_BuyUpgradeButton.interactable = true;
                    }
                    else
                    {
                        m_NotEnoghtStarsText.gameObject.SetActive(true);
                        m_BuyUpgradeButton.GetComponent<Image>().color = Color.red;
                        m_BuyUpgradeButton.interactable = false;
                    }
                }
            }

            if (upgradeSlot.SlotType == SlotType.TowerStatsInfo)
            {
                m_MaxLevelImage.gameObject.SetActive(false);
                m_StarImage.gameObject.SetActive(false);
                m_BuyUpgradeButton.gameObject.SetActive(false);
                m_NotEnoghtStarsText.gameObject.SetActive(false);
                m_SelectedSlotLvlInfoText.gameObject.SetActive(true);
                m_LeftArrowButton.gameObject.SetActive(true);
                m_RightArrowButton.gameObject.SetActive(true);

                m_SelectedUpgradeIconImage.sprite = upgradeSlot.TowerSettings[m_SelectedSlotInfoIndex].TowerGUISprite;

                if (m_SelectedSlotInfoIndex == 0)
                    m_LeftArrowButton.interactable = false;
                else
                    m_LeftArrowButton.interactable = true;

                if (m_SelectedSlotInfoIndex == upgradeSlot.TowerSettings.Length - 1)
                    m_RightArrowButton.interactable = false;
                else
                    m_RightArrowButton.interactable = true;

                m_SelectedSlotLvlInfoText.text = "lvl " + (m_SelectedSlotInfoIndex + 1); // ������
                m_SelectedUpgradeDescriptionText.text = "";

                if (upgradeSlot.TowerSettings[m_SelectedSlotInfoIndex].TurretProps != null)
                {
                    float radius = upgradeSlot.TowerSettings[m_SelectedSlotInfoIndex].Radius + Upgrades.GetUpgradeValueByLevel(upgradeSlot.TowerSettings[m_SelectedSlotInfoIndex].AttackRangeUpgrade, Upgrades.GetUpgradeLevel(upgradeSlot.TowerSettings[m_SelectedSlotInfoIndex].AttackRangeUpgrade));
                    int damage = upgradeSlot.TowerSettings[m_SelectedSlotInfoIndex].TurretProps.ProjectilePrefab.Damage + (int)Upgrades.GetUpgradeValueByLevel(upgradeSlot.TowerSettings[m_SelectedSlotInfoIndex].DamageUpgrade, Upgrades.GetUpgradeLevel(upgradeSlot.TowerSettings[m_SelectedSlotInfoIndex].DamageUpgrade));
                    float attackSpeed = upgradeSlot.TowerSettings[m_SelectedSlotInfoIndex].TurretProps.FireRate - Upgrades.GetUpgradeValueByLevel(upgradeSlot.TowerSettings[m_SelectedSlotInfoIndex].AttackSpeedUpgrade, Upgrades.GetUpgradeLevel(upgradeSlot.TowerSettings[m_SelectedSlotInfoIndex].AttackSpeedUpgrade));

                    m_SelectedUpgradeDescriptionText.text += "Attack Range " + radius + "\n" +
                                                        "Damage " + damage + "\n" +
                                                        "Attack Speed " + attackSpeed + "s";
                }

                if (upgradeSlot.TowerSettings[m_SelectedSlotInfoIndex].SpawnUnits)
                {
                    int unitsHP = upgradeSlot.TowerSettings[m_SelectedSlotInfoIndex].UnitSettings.HitPoints + (int)Upgrades.GetUpgradeValueByLevel(upgradeSlot.TowerSettings[m_SelectedSlotInfoIndex].UnitsHitPointsUpgrade, Upgrades.GetUpgradeLevel(upgradeSlot.TowerSettings[m_SelectedSlotInfoIndex].UnitsHitPointsUpgrade));
                    int unitsDamage = upgradeSlot.TowerSettings[m_SelectedSlotInfoIndex].UnitSettings.MeleeDamage + (int)Upgrades.GetUpgradeValueByLevel(upgradeSlot.TowerSettings[m_SelectedSlotInfoIndex].UnitsDamageUpgrade, Upgrades.GetUpgradeLevel(upgradeSlot.TowerSettings[m_SelectedSlotInfoIndex].UnitsDamageUpgrade));
                    float unitsRespawnTime = upgradeSlot.TowerSettings[m_SelectedSlotInfoIndex].UnitsRespawnTime - Upgrades.GetUpgradeValueByLevel(upgradeSlot.TowerSettings[m_SelectedSlotInfoIndex].UnitsRespawnTimeUpgrade, Upgrades.GetUpgradeLevel(upgradeSlot.TowerSettings[m_SelectedSlotInfoIndex].UnitsRespawnTimeUpgrade));

                    m_SelectedUpgradeDescriptionText.text += "Soldiers Health " + unitsHP + "\n" +
                                                             "Soldiers Damage " + unitsDamage + "\n" + 
                                                             "Soldiers Respawn Time " + unitsRespawnTime + "s";
                }

                int cost = upgradeSlot.TowerSettings[m_SelectedSlotInfoIndex].GoldCost;
                m_SelectedUpgradeDescriptionText.text += "\n" + "Gold Cost " + cost;
            }
        }

        public void BuyUpgrade()
        {
            Upgrades.BuyUpgrade(m_SelectedUpgrade.UpgradeAsset);

            UpdateStars();
            m_SelectedUpgrade.UpdateSlot();
            SelectUpgradeSlot(m_SelectedUpgrade);
        }

        public void LeftArrowButton()
        {
            m_SelectedSlotInfoIndex--;
            SelectUpgradeSlot(m_SelectedUpgrade);
        }

        public void RightArrowButton()
        {
            m_SelectedSlotInfoIndex++;
            SelectUpgradeSlot(m_SelectedUpgrade);
        }

        public void ResetUpgrades()
        {
            Upgrades.Reset();

            UpdateStars();
            foreach (var slot in m_UpgradeSlots)
                slot.UpdateSlot();

            SelectUpgradeSlot(m_SelectedUpgrade);
        }
    }
}