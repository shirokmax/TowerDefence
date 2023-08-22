using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class UIBuyControl : MonoBehaviour
    {
        private const int TBC_OFFSET = 110;

        [SerializeField] private UITowerBuyControl m_TowerBuyControlPrefab;
        [SerializeField] private UITowerSellControl m_TowerSellControlPrefab;

        [Space]
        [SerializeField] private ImpactEffect m_SelectBuildSpotSFXPrefab;

        private List<UITowerBuyControl> m_ActiveTowerBuyControls;
        private UITowerSellControl m_TowerSellControl;

        private RectTransform m_RectTransform;
        private CanvasScaler m_HUDScaler;

        private void Awake()
        {
            m_ActiveTowerBuyControls = new List<UITowerBuyControl>();
            m_HUDScaler = GetComponentInParent<CanvasScaler>();
            m_RectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            ClickSpot.EventOnSpotClick.AddListener(MoveToBuildSpot);

            gameObject.SetActive(false);
        }

        //TODO:  нопка смены holdPosition дл€ башни юнитов
        private void MoveToBuildSpot(ClickSpot clickSpot)
        {
            foreach (var towerBuyControl in m_ActiveTowerBuyControls)
                Destroy(towerBuyControl.gameObject);

            m_ActiveTowerBuyControls.Clear();

            if (m_TowerSellControl != null)
                Destroy(m_TowerSellControl.gameObject);

            if (clickSpot != null)
            {
                Vector2 pos = Camera.main.WorldToScreenPoint(clickSpot.transform.root.position);

                float sizeNormal = m_HUDScaler.referenceResolution.x / Screen.width;
                Vector2 posNormalized = new Vector2(pos.x * sizeNormal, pos.y * sizeNormal);

                float offsetMult = Screen.width / m_HUDScaler.referenceResolution.x;

                m_RectTransform.anchoredPosition = posNormalized;

                gameObject.SetActive(true);

                if ((clickSpot as BuildSpot).BuildableTowers.Length > 0)
                {
                    foreach (var towerSettings in (clickSpot as BuildSpot).BuildableTowers)
                    {
                        var newTowerBuyControl = Instantiate(m_TowerBuyControlPrefab, transform);
                        m_ActiveTowerBuyControls.Add(newTowerBuyControl);
                        newTowerBuyControl.SetTowerSettings(towerSettings);
                    }

                    float angle = 360 / m_ActiveTowerBuyControls.Count;                   

                    Vector3 startDir = Vector3.up;
                    if (m_ActiveTowerBuyControls.Count == 2) startDir = Vector3.left;
                    if (m_ActiveTowerBuyControls.Count == 4) startDir = new Vector3(-1, 1, 0).normalized;

                    for (int i = 0; i < m_ActiveTowerBuyControls.Count; i++)
                    {
                        Vector3 offset = Quaternion.AngleAxis(-angle * i, Vector3.forward) * (startDir * (TBC_OFFSET * offsetMult));
                        m_ActiveTowerBuyControls[i].transform.position += offset;
                    }
                }

                if (clickSpot.transform.root.TryGetComponent(out Tower tower))
                {
                    m_TowerSellControl = Instantiate(m_TowerSellControlPrefab, transform);
                    m_TowerSellControl.SetCurrentTower(tower);
                    Vector3 offset = Vector3.down * (TBC_OFFSET * offsetMult);
                    m_TowerSellControl.transform.position += offset;
                }

                foreach (var tbc in GetComponentsInChildren<UITowerBuyControl>())
                    tbc.SetBuildSpot(clickSpot as BuildSpot);

                if (m_SelectBuildSpotSFXPrefab != null)
                    Instantiate(m_SelectBuildSpotSFXPrefab);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
