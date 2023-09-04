using UnityEngine;

namespace TowerDefence
{
    public class TowerVisualModel : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer m_TowerSprite;
        [SerializeField] private SpriteRenderer m_GroundSprite;
        [SerializeField] private SpriteRenderer m_ShadowSprite;

        public void ApplySettings(TowerSettings settings)
        {
            m_TowerSprite.sprite = settings.TowerSprite;
            m_TowerSprite.color = settings.TowerSpriteColor;
            m_TowerSprite.transform.localScale = new Vector3(settings.TowerSpriteScale.x, settings.TowerSpriteScale.y, 1);

            m_ShadowSprite.transform.localScale = new Vector3(settings.ShadowSpriteScale.x, settings.ShadowSpriteScale.y, 1);
            m_ShadowSprite.transform.localPosition = new Vector3(settings.ShadowPosition.x, settings.ShadowPosition.y, 0);

            m_GroundSprite.transform.localScale = new Vector3(settings.GroundSpriteScale.x, settings.GroundSpriteScale.y, 1);
            m_GroundSprite.transform.localPosition = new Vector3(settings.GroundPosition.x, settings.GroundPosition.y, 0);
        }
    }
}
