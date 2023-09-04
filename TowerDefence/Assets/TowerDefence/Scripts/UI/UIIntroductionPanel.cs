using UnityEngine;

namespace TowerDefence
{
    public class UIIntroductionPanel : MonoBehaviour
    {
        [SerializeField] private GameObject m_IntroductionPanel;

        private void Start()
        {
            if (PlayerPrefs.GetInt("Settings:IntroductionPanel", 0) == 0)
            {
                m_IntroductionPanel.SetActive(true);
                PlayerPrefs.SetInt("Settings:IntroductionPanel", 1);
            }
            else
            {
                m_IntroductionPanel.SetActive(false);
            }
        }
    }
}
