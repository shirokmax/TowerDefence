using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TowerDefence
{
    public class UIMainMenu : MonoBehaviour
    {
        public const float DEFAULT_VOLUME_VALUE = 0.7f;

        [SerializeField] private Button m_ContinueButton;
        [SerializeField] private Image m_ContinueTextImage;
        [SerializeField] private Color m_ContinueTextInactiveColor;

        [Space]
        [SerializeField] private GameObject m_StartNewGamePanel;

        private void Awake()
        {
            AudioListener.volume = PlayerPrefs.GetFloat("Settings:Volume", DEFAULT_VOLUME_VALUE);
        }

        private void Start()
        {
            if (FileHandler.HasFile(MapCompletion.FILENAME) == true)
            {
                m_ContinueButton.interactable = true;
                m_ContinueTextImage.color = Color.white;
            }
            else
            {
                m_ContinueButton.interactable = false;
                m_ContinueTextImage.color = m_ContinueTextInactiveColor;
            }
        }

        public void OnNewGameButtonClick()
        {
            if (FileHandler.HasFile(MapCompletion.FILENAME) == true)
                m_StartNewGamePanel.SetActive(true);
            else
                SceneManager.LoadScene(1);
        }

        public void OnStartNewGameButtonClick()
        {
            FileHandler.Reset(MapCompletion.FILENAME);
            FileHandler.Reset(Upgrades.FILENAME);

            MapCompletion.Instance.ResetProgress();

            SceneManager.LoadScene(1);
        }

        public void OnContinueButtonClick()
        {
            SceneManager.LoadScene(1);
        }

        public void OnQuitButtonClick()
        {
            Application.Quit();
        }
    }
}
