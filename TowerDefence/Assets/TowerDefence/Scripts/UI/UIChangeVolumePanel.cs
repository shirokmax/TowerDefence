using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class UIChangeVolumePanel : MonoBehaviour
    {
        [SerializeField] private Slider m_Slider;

        private void Awake()
        {
            m_Slider.value = PlayerPrefs.GetFloat("Settings:Volume");
        }

        public void OnVolumeChange()
        {
            AudioListener.volume = m_Slider.value;
            PlayerPrefs.SetFloat("Settings:Volume", m_Slider.value);
        }
    }
}
