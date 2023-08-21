using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class UIBuyControl : MonoBehaviour
    {
        private const int TBC_OFFSET = 110;

        [SerializeField] private UITowerBuyControl m_TowerBuyControlPrefab;

        [Space]
        [SerializeField] private ImpactEffect m_SelectBuildSpotSFXPrefab;

        private List<UITowerBuyControl> m_ActiveTowerBuyControls;

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

        //TODO: Кнопка смены holdPosition для башни юнитов
        private void MoveToBuildSpot(ClickSpot clickSpot)
        {
            foreach (var towerBuyControl in m_ActiveTowerBuyControls)
                Destroy(towerBuyControl.gameObject);

            m_ActiveTowerBuyControls.Clear();

            if (clickSpot != null)
            {
                Vector2 pos = Camera.main.WorldToScreenPoint(clickSpot.transform.root.position);

                float sizeNormal = m_HUDScaler.referenceResolution.x / Screen.width;
                Vector2 posNormalized = new Vector2(pos.x * sizeNormal, pos.y * sizeNormal);

                m_RectTransform.anchoredPosition = posNormalized;

                gameObject.SetActive(true);

                foreach (var towerSettings in (clickSpot as BuildSpot).BuildableTowers)
                {
                    var newTowerBuyControl = Instantiate(m_TowerBuyControlPrefab, transform);
                    m_ActiveTowerBuyControls.Add(newTowerBuyControl);
                    newTowerBuyControl.SetTowerSettings(towerSettings);
                }

                // TODO: Убрать if, т.к. всегда будет отображаться кнопка продажи?
                if (m_ActiveTowerBuyControls.Count > 0)
                {
                    float angle = 360 / m_ActiveTowerBuyControls.Count;
                    float offsetMult = Screen.width / m_HUDScaler.referenceResolution.x;

                    Vector3 startDir = Vector3.up;
                    if (m_ActiveTowerBuyControls.Count == 2) startDir = Vector3.left;
                    if (m_ActiveTowerBuyControls.Count == 4) startDir = new Vector3(-1, 1, 0).normalized;

                    for (int i = 0; i < m_ActiveTowerBuyControls.Count; i++)
                    {
                        Vector3 offset = Quaternion.AngleAxis(-angle * i, Vector3.forward) * (startDir * (TBC_OFFSET * offsetMult));
                        m_ActiveTowerBuyControls[i].transform.position += offset;
                    }
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
