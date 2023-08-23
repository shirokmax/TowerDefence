using UnityEngine;
using UnityEngine.UI;
using SpaceShooter;

namespace TowerDefence
{
    public class UITowerSellControl : MonoBehaviour
    {
        [SerializeField] private Button m_BuyButton;
        [SerializeField] private Text m_GoldText;
        [SerializeField] private GameObject m_BuildSpotPrefab;

        [Space]
        [Range(0f, 1f)]
        [SerializeField] private float m_TowerSellGoldValueRate = 0.6f;

        private Tower m_CurrentTower;

        private int m_SellGoldValue;

        private void Start()
        {
            m_SellGoldValue = CalculateSellGoldValue();

            m_GoldText.text = m_SellGoldValue.ToString();
        }

        public void Sell()
        {
            Player.Instance.AddGold(m_SellGoldValue);
            Instantiate(m_BuildSpotPrefab, m_CurrentTower.transform.position, Quaternion.identity);
            ClickSpot.EventOnSpotClick.Invoke(null);
            Destroy(m_CurrentTower.gameObject);
        }

        private int CalculateSellGoldValue()
        {
            if (EnemyWavesManager.Instance.WavesStarted == true)
                return (int)(m_CurrentTower.TotalCost * m_TowerSellGoldValueRate);

            return m_CurrentTower.TotalCost;
        }

        public void SetCurrentTower(Tower tower)
        {
            m_CurrentTower = tower;
        }
    }
}
