using SpaceShooter;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public enum SourceType
    {
        Gold,
        Lives
    }

    [RequireComponent(typeof(Text))]
    public class UISourceTextChange : MonoBehaviour
    {
        [SerializeField] private SourceType m_SoureType;

        private Text m_Text;

        private Animator m_Animator;

        private void Awake()
        {
            m_Text = GetComponent<Text>();
            m_Animator = GetComponent<Animator>();
        }

        private void Start()
        {
            switch (m_SoureType)
            {
                case SourceType.Gold:
                    Player.Instance.GoldChangeSubscribe(OnGoldChange);
                    break;

                case SourceType.Lives:
                    {
                        Player.Instance.LivesChangeSubscribe(OnLivesChange);
                        if (m_Animator != null)
                            Player.Instance.EventOnTakeDamage.AddListener(OnDamagePlayer);
                    }
                    break;

                default:
                    break;
            }
        }

        private void OnGoldChange()
        {
            m_Text.text = Player.Instance.Gold.ToString();
        }

        private void OnLivesChange()
        {
            m_Text.text = Player.Instance.NumLives.ToString();
        }

        private void OnDamagePlayer()
        {
            m_Animator.Play("TakeDamage@LivesText");
        }
    }
}
