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
        [SerializeField] protected int m_ManaCost;

        protected bool m_InCoolDown;

        private void Start()
        {
            Player.Instance.ManaChangeSubscribe(OnManaChange);

            m_CostText.text = m_ManaCost.ToString();
        }

        private void OnManaChange()
        {
            if (Player.Instance.Mana < m_ManaCost || m_InCoolDown)
                m_UseSpellButton.interactable = false;
            else
                m_UseSpellButton.interactable = true;

            if (Player.Instance.Mana < m_ManaCost)
                m_CostText.color = Color.red;
            else
                m_CostText.color = Color.white;
        }

        public abstract void Use();
    }
}
