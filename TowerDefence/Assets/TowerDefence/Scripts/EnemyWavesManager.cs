using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using SpaceShooter;
using UnityEngine.UI;

namespace TowerDefence
{
    public class EnemyWavesManager : MonoSingleton<EnemyWavesManager>
    {
        [SerializeField] private Enemy m_EnemyPrefab;
        [SerializeField] private UnitPath[] m_Paths;

        [Space]
        [SerializeField] private int m_SkipWaveBonusGoldPerSecond = 5;
        public int SkipWaveBonusGoldPerSecond => m_SkipWaveBonusGoldPerSecond;

        [Space]
        [SerializeField] private ImpactEffect m_WaveStartSFXPrefab;

        private EnemyWave[] m_Waves;
        private int m_WaveIndex;

        private UnityEvent<int, int> m_EventOnWaveNumChange = new UnityEvent<int, int>();

        private UnityEvent m_EventOnStartSpawnEnemies = new UnityEvent();
        public UnityEvent EventOnStartSpawnEnemies => m_EventOnStartSpawnEnemies;

        private UnityEvent m_EventOnEndSpawnEnemies = new UnityEvent();
        public UnityEvent EventOnEndSpawnEnemies => m_EventOnEndSpawnEnemies;

        public void WaveNumChangeSubscribe(UnityAction<int, int> action)
        {
            m_EventOnWaveNumChange.AddListener(action);
            m_EventOnWaveNumChange.Invoke(m_WaveIndex, m_Waves.Length);
        }

        protected override void Awake()
        {
            base.Awake();

            //����� ����� �� ���� �������� ��������
            m_Waves = GetComponentsInChildren<EnemyWave>();
        }

        private void Start()
        {
            if (m_Waves.Length > 0)
                m_Waves[m_WaveIndex].Prepare(StartSpawnEnemies);
        }

        private void StartSpawnEnemies()
        {
            StartCoroutine(SpawnEnemiesCoroutine());
        }

        public void ForceWave()
        {
            if (m_WaveIndex >= m_Waves.Length) return;

            Player.Instance.AddGold((int)m_Waves[m_WaveIndex].PrepareRemainingTime * m_SkipWaveBonusGoldPerSecond);

            m_Waves[m_WaveIndex].CancelPrepare();
            StartCoroutine(SpawnEnemiesCoroutine());
        }

        private IEnumerator SpawnEnemiesCoroutine()
        {
            //���������� ������ ������������ ����. �����
            m_EventOnStartSpawnEnemies?.Invoke();

            m_EventOnWaveNumChange.Invoke(m_WaveIndex + 1, m_Waves.Length);

            if (m_WaveStartSFXPrefab != null)
                Instantiate(m_WaveStartSFXPrefab);

            foreach ((UnitSettings settings, int count, int pathIndex) in m_Waves[m_WaveIndex].EnumerateSquads())
            {
                if (pathIndex < m_Paths.Length)
                {
                    for (int i = 0; i < count; i++)
                    {
                        Enemy enemy = Instantiate(m_EnemyPrefab, m_Paths[pathIndex].StartArea.GetRandomInsideZone(), Quaternion.identity);
                        enemy.ApplySettings(settings);
                        enemy.GetComponent<AIController>().SetPathBehaviour(m_Paths[pathIndex], AIBehaviour.PathMove);

                        yield return new WaitForSeconds(m_Waves[m_WaveIndex].DelayBetweenSpawn);
                    }
                }
                else
                {
                    Debug.LogWarning($"Invalid pathIndex in {name}");
                }
            }

            m_WaveIndex++;

            if (m_WaveIndex < m_Waves.Length)
            {
                m_Waves[m_WaveIndex].Prepare(StartSpawnEnemies);
                //��������� ������ ������������ ����. �����
                m_EventOnEndSpawnEnemies?.Invoke();
            }
        }
    }
}
