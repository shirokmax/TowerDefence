using UnityEngine;
using UnityEngine.UI;
using SpaceShooter;

namespace TowerDefence
{
    public class UITowerSellControl : MonoBehaviour
    {
        [SerializeField] private Button m_BuyButton;
        [SerializeField] private Text m_GoldText;

        [Space]
        [SerializeField] private GameObject m_BuildSpotPrefab;

        private Tower m_CurrentTower;

        private void Start()
        {
            m_GoldText.text = m_CurrentTower.TotalCost.ToString();
        }

        public void Sell()
        {
            Player.Instance.AddGold(m_CurrentTower.TotalCost);
            Instantiate(m_BuildSpotPrefab, m_CurrentTower.transform.position, Quaternion.identity);
            ClickSpot.EventOnSpotClick.Invoke(null);
            Destroy(m_CurrentTower.gameObject);
        }

        public void SetCurrentTower(Tower tower)
        {
            m_CurrentTower = tower;
        }
    }
}
