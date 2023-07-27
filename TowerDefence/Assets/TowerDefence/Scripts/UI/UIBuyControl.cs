using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class UIBuyControl : MonoBehaviour
    {
        [SerializeField] private ImpactEffect m_SelectBuildSpotSFXPrefab;

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
            if (buildSpot != null)
            {
                Vector2 pos = Camera.main.WorldToScreenPoint(buildSpot.position);

                float xSizeNormal = m_HUDScaler.referenceResolution.x / Screen.width;
                float ySizeNormal = m_HUDScaler.referenceResolution.y / Screen.height;
                Vector2 posNormalized = new Vector2(pos.x * xSizeNormal, pos.y * ySizeNormal);

                m_RectTransform.anchoredPosition = posNormalized;

                gameObject.SetActive(true);

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
