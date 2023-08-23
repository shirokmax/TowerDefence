using UnityEngine;
using UnityEngine.UI;
using SpaceShooter;

namespace TowerDefence
{
    public abstract class MagicSpell : MonoBehaviour
    {
        [SerializeField] protected Button m_UseSpellButton;
        [SerializeField] protected Text m_CostText;

        [Space]
        [SerializeField] protected Sprite m_DefaultSpellSprite;
        [SerializeField] protected Sprite m_ActiveSpellSprite;

        [Space]
        [SerializeField] protected int m_ManaCost;
        [SerializeField] protected float m_Duration;

        protected Image m_UseSpellButtonImage;

        protected bool m_IsSpellActive;

        private void Awake()
        {
            m_UseSpellButtonImage = m_UseSpellButton.transform.GetComponent<Image>();
            m_UseSpellButtonImage.sprite = m_DefaultSpellSprite;
        }

        private void Start()
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
    }
}
