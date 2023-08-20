using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class UIBuyControl : MonoBehaviour
    {
        [SerializeField] private UITowerBuyControl m_TowerBuyControlPrefab;
        [SerializeField] private TowerSettings[] m_TowersSettings;
        [SerializeField] private int m_TowerBuyControlsOffset = 80;

        [Space]
        [SerializeField] private ImpactEffect m_SelectBuildSpotSFXPrefab;

        private List<UITowerBuyControl> m_ActiveTowerBuyControls;

        private RectTransform m_RectTransform;
        private CanvasScaler m_HUDScaler;

        private void Awake()
        {
            m_HUDScaler = GetComponentInParent<CanvasScaler>();
            m_RectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            ClickSpot.EventOnSpotClick.AddListener(MoveToBuildSpot);

            gameObject.SetActive(false);
        }

        //TODO:  нопка смены holdPosition дл€ башни юнитов
        private void MoveToBuildSpot(Transform buildSpot)
        {
            if (m_ActiveTowerBuyControls != null)
            {
                foreach (var towerBuyControl in m_ActiveTowerBuyControls)
                    Destroy(towerBuyControl.gameObject);
            }

            m_ActiveTowerBuyControls = new List<UITowerBuyControl>();

            if (buildSpot != null)
            {
                Vector2 pos = Camera.main.WorldToScreenPoint(buildSpot.position);

                float xSizeNormal = m_HUDScaler.referenceResolution.x / Screen.width;
                float ySizeNormal = m_HUDScaler.referenceResolution.y / Screen.height;
                Vector2 posNormalized = new Vector2(pos.x * xSizeNormal, pos.y * ySizeNormal);

                m_RectTransform.anchoredPosition = posNormalized;

                gameObject.SetActive(true);

                for (int i = 0; i < m_TowersSettings.Length; i++)
                {
                    var newTowerBuyControl = Instantiate(m_TowerBuyControlPrefab, transform);

                    m_ActiveTowerBuyControls.Add(newTowerBuyControl);
                    newTowerBuyControl.SetTowerSettings(m_TowersSettings[i]);
                }

                float angle = 360 / m_ActiveTowerBuyControls.Count;
                Vector3 startDir = Vector3.up;
                if (m_ActiveTowerBuyControls.Count == 2) startDir = Vector3.left;
                if (m_ActiveTowerBuyControls.Count == 4) startDir = new Vector3(-1, 1, 0).normalized;

                for (int i = 0; i < m_ActiveTowerBuyControls.Count; i++)
                {
                    Vector3 offset = Quaternion.AngleAxis(-angle * i, Vector3.forward) * (startDir * m_TowerBuyControlsOffset);
                    m_ActiveTowerBuyControls[i].transform.position += offset;
                }

                foreach (var tbc in GetComponentsInChildren<UITowerBuyControl>())
                    tbc.SetBuildSpot(buildSpot);

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
