using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class UICallNextWave : MonoBehaviour
    {
        [Space]
        [SerializeField] private Button m_SkipWaveButton;
        [SerializeField] private GameObject WaveSkipBonusPanel;
        [SerializeField] private GameObject WaveSkipProgressBarPanel;

        [Space]
        [SerializeField] private Text WaveSkipBonusGoldText;
        [SerializeField] private Image WaveSkipProgressBarImage;

        private float m_TimeToNextWave;

        private float m_NextWaveTimer;

        private void Start()
        {
            enabled = false;

            EnemyWavesManager.Instance.EventOnStartSpawnEnemies.AddListener(OnSkipDisable);
            EnemyWavesManager.Instance.EventOnEndSpawnEnemies.AddListener(OnSkipEnable);

            EnemyWave.EventOnWavePrepare.AddListener((float time) =>
            {
                m_TimeToNextWave = time;
                m_NextWaveTimer = time;
            });

            WaveSkipProgressBarPanel.gameObject.SetActive(false);
            WaveSkipBonusGoldText.text = " START WAVE";
        }

        private void Update()
        {
            float fill = m_NextWaveTimer / m_TimeToNextWave;
            WaveSkipProgressBarImage.fillAmount = fill;
            WaveSkipProgressBarImage.color = new Color(1, fill, 0f);

            int bonus = (int)m_NextWaveTimer * EnemyWavesManager.Instance.SkipWaveBonusGoldPerSecond;
            WaveSkipBonusGoldText.text = $"Wave skip bonus:{bonus}";

            if (m_NextWaveTimer > 0)
                m_NextWaveTimer -= Time.deltaTime;
        }

        public void OnCallWaveButtonClick()
        {
            enabled = true;
            ClickSpot.EventOnSpotClick.Invoke(null);
            EnemyWavesManager.Instance?.ForceWave();
        }

        private void OnSkipDisable()
        {
            m_SkipWaveButton.gameObject.SetActive(false);
            WaveSkipBonusPanel.gameObject.SetActive(false);
            WaveSkipProgressBarPanel.gameObject.SetActive(false);
        }

        private void OnSkipEnable()
        {
            m_SkipWaveButton.gameObject.SetActive(true);
            WaveSkipBonusPanel.gameObject.SetActive(true);
            WaveSkipProgressBarPanel.gameObject.SetActive(true);
        }
    }
}
