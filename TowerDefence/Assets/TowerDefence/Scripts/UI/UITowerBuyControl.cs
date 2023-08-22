using UnityEngine;
using UnityEngine.UI;
using SpaceShooter;

namespace TowerDefence
{
    public class UITowerBuyControl : MonoBehaviour
    {
        [SerializeField] protected TowerSettings m_TowerSettings;

        [Space]
        [SerializeField] protected Button m_BuyButton;
        [SerializeField] protected Text m_GoldText;

        protected BuildSpot m_BuildSpot;

        private void Start()
        {
            m_GoldText.text = m_TowerSettings.GoldCost.ToString();
            m_BuyButton.GetComponent<Image>().sprite = m_TowerSettings.TowerGUISprite;

            Player.Instance.GoldChangeSubscribe(GoldStatusCheck);
        }

        private void GoldStatusCheck()
        {
            if (Player.Instance.Gold >= m_TowerSettings.GoldCost != m_BuyButton.interactable)
            {
                m_BuyButton.interactable = !m_BuyButton.interactable;

                m_GoldText.color = m_BuyButton.interactable ? Color.white : Color.red;
            }
        }

        public void Buy()
        {
            Player.Instance.TryBuild(m_TowerSettings, m_BuildSpot);
        }

        public void SetBuildSpot(BuildSpot spot)
        {
            m_BuildSpot = spot;
        }

        public void SetTowerSettings(TowerSettings settings)
        {
            m_TowerSettings = settings;
        }
    }
}
