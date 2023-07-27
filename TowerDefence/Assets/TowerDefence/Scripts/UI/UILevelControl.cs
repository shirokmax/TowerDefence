using SpaceShooter;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class UILevelControl : MonoBehaviour
    {
        private RectTransform m_RectTransform;
        private CanvasScaler m_Canvas;

        private Episode m_selectedEpisode;

        private void Awake()
        {
            m_Canvas = GetComponentInParent<CanvasScaler>();
            m_RectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            ClickSpot.EventOnSpotClick.AddListener(MoveToLevelSpot);

            gameObject.SetActive(false);
        }

        private void MoveToLevelSpot(Transform levelSpot)
        {
            if (levelSpot != null)
            {
                Vector2 pos = Camera.main.WorldToScreenPoint(levelSpot.position);

                float xSizeNormal = m_Canvas.referenceResolution.x / Screen.width;
                float ySizeNormal = m_Canvas.referenceResolution.y / Screen.height;
                Vector2 posNormalized = new Vector2(pos.x * xSizeNormal, pos.y * ySizeNormal);

                m_RectTransform.anchoredPosition = posNormalized;

                m_selectedEpisode = levelSpot.root.GetComponent<MapLevel>().Episode;

                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void OnPlayButtonClick()
        {
            if (m_selectedEpisode == null) return;

            LevelSequenceController.Instance.StartEpisode(m_selectedEpisode);
        }
    }
}
