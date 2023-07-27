using UnityEngine;
using SpaceShooter;
using UnityEngine.UI;

namespace TowerDefence
{
    public class MapLevel : MonoBehaviour
    {
        private Episode m_Epidode;
        public Episode Episode => m_Epidode;

        [Space]
        [SerializeField] private SpriteRenderer m_LevelSelectSpriteRenderer;

        [SerializeField] private Sprite m_LevelDefaultSprite;
        [SerializeField] private Sprite m_LevelSelectedSprite;

        [Space]
        [SerializeField] private ImpactEffect m_SelectLevelSFX;
        [SerializeField] private ImpactEffect m_MissclickLevelSFX;

        [Space]
        [SerializeField] private Image[] m_Stars;
        [SerializeField] private Color m_DeactivatedStarColor = new Color(0, 0, 0, 0.82f);

        private bool m_LevelSelected;

        private void Awake()
        {
            m_LevelSelectSpriteRenderer.sprite = m_LevelDefaultSprite;

            foreach (var star in m_Stars)
                star.color = m_DeactivatedStarColor;
        }

        private void Start()
        {
            ClickSpot.EventOnSpotClick.AddListener(OnLevelClicked);
        }

        public void SetLevelData(Episode episode, int stars)
        {
            m_Epidode = episode;

            if (stars <= m_Stars.Length)
            {
                for (int i = 0; i < stars; i++)
                    m_Stars[i].color = Color.white;
            }
        }

        private void OnLevelClicked(Transform mapLevel)
        {
            if (mapLevel == null)
            {
                if (m_LevelSelected == true)
                {
                    SelectLevel(false);

                    if (m_MissclickLevelSFX != null)
                        Instantiate(m_MissclickLevelSFX);
                }

                return;
            }

            if (transform.root == mapLevel)
            {
                if (m_LevelSelected == false)
                {
                    SelectLevel(true);

                    if (m_SelectLevelSFX != null)
                        Instantiate(m_SelectLevelSFX);
                }
            }
            else
            {
                if (m_LevelSelected == true)
                    SelectLevel(false);
            }
        }

        private void SelectLevel(bool select)
        {
            if (select == true)
            {
                m_LevelSelected = true;
                m_LevelSelectSpriteRenderer.sprite = m_LevelSelectedSprite;
            }
            else
            {
                m_LevelSelected = false;
                m_LevelSelectSpriteRenderer.sprite = m_LevelDefaultSprite;
            }
        }
    }
}
