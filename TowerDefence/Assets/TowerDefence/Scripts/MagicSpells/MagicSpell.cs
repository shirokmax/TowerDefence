using UnityEngine;
using UnityEngine.UI;
using SpaceShooter;

namespace TowerDefence
{
    public abstract class MagicSpell : MonoBehaviour
    {
        [SerializeField] protected Button m_UseSpellButton;
        [SerializeField] protected Text m_CostText;
        [SerializeField] protected Sprite m_DefaultSpellIconSprite;
        [SerializeField] protected Sprite m_ActiveSpellIconSprite;
        [SerializeField] protected Text m_magicSpellNameText;
        [SerializeField] protected Image m_ManaCostPanelImage;
        [SerializeField] protected Image m_ManaImage;
        [SerializeField] protected Color m_InactiveColor;

        [Space]
        [SerializeField] protected MagicSpellProperties m_Properties;

        [Space]
        [SerializeField] protected int m_ManaCost;
        [SerializeField] protected float m_Duration;

        protected Image m_UseSpellButtonImage;

        protected bool m_IsSpellActive;

        protected virtual void Awake()
        {
            m_UseSpellButtonImage = m_UseSpellButton.transform.GetComponent<Image>();
            m_UseSpellButtonImage.sprite = m_DefaultSpellIconSprite;
        }

        protected virtual void Start()
        {
            Player.Instance.ManaChangeSubscribe(OnManaChange);
            m_CostText.text = m_ManaCost.ToString();
        }

        private void OnManaChange()
        {
            if (Player.Instance.Mana < m_ManaCost || m_IsSpellActive)
                m_UseSpellButton.interactable = false;
            else
                m_UseSpellButton.interactable = true;

            if (Player.Instance.Mana < m_ManaCost)
                m_CostText.color = Color.red;
            else
                m_CostText.color = Color.white;
        }

        public virtual void Use()
        {
            ClickSpot.EventOnSpotClick.Invoke(null);
        }

        public virtual void ApplyProperties(MagicSpellProperties props)
        {
            m_DefaultSpellIconSprite = props.DefaultSpellIconSprite;
            m_ActiveSpellIconSprite = props.ActiveSpellIconSprite;

            m_ManaCost = props.ManaCost;
            m_Duration = props.Duration;
        }
    }
}
