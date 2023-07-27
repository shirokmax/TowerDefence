using SpaceShooter;
using UnityEngine;

namespace TowerDefence
{
    [RequireComponent (typeof(AudioSource))]
    public class BGMusic : MonoSingleton<BGMusic>
    {
        private AudioSource m_Music;

        [SerializeField] private float m_EndLevelVolume = 0.05f;

        protected override void Awake()
        {
            base.Awake();

            m_Music = GetComponent<AudioSource>();
        }

        private void Start()
        {
            LevelController.Instance.EventLevelComplete.AddListener(MusicVolumeDown);
        }

        private void MusicVolumeDown()
        {
            m_Music.volume = m_EndLevelVolume;
        }
    }
}
