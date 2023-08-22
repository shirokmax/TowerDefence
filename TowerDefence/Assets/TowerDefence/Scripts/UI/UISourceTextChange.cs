using SpaceShooter;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public enum SourceType
    {
        Gold,
        Mana,
        Lives,
        Waves
    }

    [RequireComponent(typeof(Text))]
    public class UISourceTextChange : MonoBehaviour
    {
        [SerializeField] private SourceType m_SoureType;
        [SerializeField] private Image m_FillImage;

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

                case SourceType.Mana:
                    Player.Instance.ManaChangeSubscribe(OnManaChange);
                    break;

                case SourceType.Lives:
                    {
                        Player.Instance.LivesChangeSubscribe(OnLivesChange);
                        if (m_Animator != null)
                            Player.Instance.EventOnTakeDamage.AddListener(OnDamagePlayer);
                    }
                    break;

                case SourceType.Waves:
                    EnemyWavesManager.Instance.WaveNumChangeSubscribe(OnWaveChange);
                    break;

                default:
                    break;
            }
        }

        private void OnGoldChange()
        {
            m_Text.text = Player.Instance.Gold.ToString();
        }

        private void OnManaChange()
        {
            if (m_FillImage != null)
                m_FillImage.fillAmount = Player.Instance.Mana / Player.Instance.MaxMana;

            m_Text.text = "mana " + ((int)Player.Instance.Mana);
        }

        private void OnLivesChange()
        {
            m_Text.text = Player.Instance.NumLives.ToString();
        }

        private void OnDamagePlayer()
        {
            m_Animator.Play("TakeDamage@LivesText");
        }

        private void OnWaveChange(int num, int allCount)
        {
            m_Text.text = $"Wave {num}/{allCount}";
        }
    }
}
