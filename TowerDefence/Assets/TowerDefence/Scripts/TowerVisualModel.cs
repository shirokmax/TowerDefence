using UnityEngine;

namespace TowerDefence
{
    public class TowerVisualModel : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer m_TowerSprite;
        [SerializeField] private SpriteRenderer m_ShadowSprite;

        public void ApplySettings(TowerSettings settings)
        {
            m_TowerSprite.sprite = settings.TowerSprite;
            m_TowerSprite.color = settings.TowerSpriteColor;
            m_TowerSprite.transform.localScale = new Vector3(settings.TowerSpriteScale.x, settings.TowerSpriteScale.y, 1);

            m_ShadowSprite.transform.localScale = new Vector3(settings.ShadowSpriteScale.x, settings.ShadowSpriteScale.y, 1);
            m_ShadowSprite.transform.localPosition = new Vector3(settings.ShadowPosition.x, settings.ShadowPosition.y, 0);
        }
    }
}
