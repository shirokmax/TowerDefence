using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class UIResultPanel : MonoSingleton<UIResultPanel>
    {
        [SerializeField] private GameObject m_LevelCompletePanel;
        [SerializeField] private GameObject m_GameOverPanel;

        [Space]
        [SerializeField] private GameObject[] m_Stars;

        [Space]
        [SerializeField] private ImpactEffect m_LevelCompleteSFXPrefab;
        [SerializeField] private ImpactEffect m_GameOverSFXPrefab;

        [Space]
        [SerializeField] private Text m_ScoreText;
        [SerializeField] private Text m_KillsText;
        [SerializeField] private Text m_TimeText;

        [Space]
        [SerializeField] private Text m_NextButtonText;

        private bool m_Success;

        private void Start()
        {
            m_LevelCompletePanel.SetActive(false);
            m_GameOverPanel.SetActive(false);

            foreach (var star in m_Stars)
                star.SetActive(false);

            gameObject.SetActive(false);
        }

        public void ShowResults(PlayerStatistics levelResults, bool succes)
        {
            gameObject.SetActive(true);

            m_Success = succes;

            //TODO: Нужно ли отображать какие-либо статы результатов?
            //m_ScoreText.text = "Score: " + levelResults.Score.ToString();
            //m_KillsText.text = "Kills: " + levelResults.Kills.ToString();
            //m_TimeText.text = "Level time: " + TimeFormat.Format(levelResults.Time);

            if (succes)
            {
                m_LevelCompletePanel.SetActive(true);

                if (levelResults.Stars <= m_Stars.Length)
                {
                    for (int i = 0; i < levelResults.Stars; i++)
                        m_Stars[i].SetActive(true);
                }

                m_NextButtonText.text = "Next";

                if (m_LevelCompleteSFXPrefab != null)
                    Instantiate(m_LevelCompleteSFXPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                m_GameOverPanel.SetActive(true);
                m_NextButtonText.text = "Restart";

                if (m_GameOverSFXPrefab != null)
                    Instantiate(m_GameOverSFXPrefab, transform.position, Quaternion.identity);
            }
        }

        public void OnNextActionButtonClick()
        {
            gameObject.SetActive(false);

            if (m_Success)
                LevelSequenceController.Instance.AdvanceLevel();
            else
                LevelSequenceController.Instance.RestartLevel();
        }

        public void OnExitMainMenuButtonClick()
        {
            LevelSequenceController.Instance.LoadMainMenu();
        }
    }
}
