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
        [Min(0)][SerializeField] private int m_SkipWaveBonusGoldPerSecond = 5;
        public int SkipWaveBonusGoldPerSecond => m_SkipWaveBonusGoldPerSecond;

        [Space]
        [SerializeField] private ImpactEffect m_WaveStartSFXPrefab;

        private static UnityEvent<Enemy> m_EventOnEnemySpawn;
        public static UnityEvent<Enemy> EventOnEnemySpawn => m_EventOnEnemySpawn;

        private EnemyWave[] m_Waves;
        private int m_WaveIndex;

        private int m_ActiveEnemyUnitsCount;

        private bool m_WavesStarted;
        public bool WavesStarted => m_WavesStarted;

        private UnityEvent<int, int> m_EventOnWaveNumChange = new UnityEvent<int, int>();

        private UnityEvent m_EventOnStartSpawnEnemies = new UnityEvent();
        public UnityEvent EventOnStartSpawnEnemies => m_EventOnStartSpawnEnemies;

        private UnityEvent m_EventOnEndSpawnEnemies = new UnityEvent();
        public UnityEvent EventOnEndSpawnEnemies => m_EventOnEndSpawnEnemies;

        private UnityEvent m_EventOnAllWavesDead = new UnityEvent();
        public UnityEvent EventOnAllWavesDead => m_EventOnAllWavesDead;

        public void WaveNumChangeSubscribe(UnityAction<int, int> action)
        {
            m_EventOnWaveNumChange.AddListener(action);
            m_EventOnWaveNumChange.Invoke(m_WaveIndex, m_Waves.Length);
        }

        protected override void Awake()
        {
            base.Awake();

            //Берем волны из всех дочерних объектов
            m_Waves = GetComponentsInChildren<EnemyWave>();

            m_EventOnEnemySpawn ??= new UnityEvent<Enemy>();
        }

        private void Start()
        {
            LevelController.Instance.SetReferenceTime(CalculateLevelReferenceTime());
        }

        private float CalculateLevelReferenceTime()
        {
            float time = 0;

            foreach (var wave in m_Waves)
                time += wave.GetWaveTime();

            return time;
        }

        private void StartSpawnEnemies()
        {
            StartCoroutine(SpawnEnemiesCoroutine());
        }

        public void ForceWave()
        {
            if (m_WaveIndex >= m_Waves.Length) return;

            if (m_WaveIndex != 0)
            {
                Player.Instance.AddGold((int)m_Waves[m_WaveIndex].PrepareRemainingTime * m_SkipWaveBonusGoldPerSecond);
                m_Waves[m_WaveIndex].CancelPrepare();
            }

            StartCoroutine(SpawnEnemiesCoroutine());
        }

        private void OnEnemyUnitDeath()
        {
            m_ActiveEnemyUnitsCount--;

            if (m_WaveIndex == m_Waves.Length && m_ActiveEnemyUnitsCount == 0)
                m_EventOnAllWavesDead?.Invoke();
        }

        private IEnumerator SpawnEnemiesCoroutine()
        {
            //Выключение кнопки форсирования след. волны
            m_EventOnStartSpawnEnemies?.Invoke();

            m_WavesStarted = true;

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

                        m_ActiveEnemyUnitsCount++;
                        enemy.EventOnDestroy.AddListener(OnEnemyUnitDeath);

                        m_EventOnEnemySpawn?.Invoke(enemy);

                        yield return new WaitForSeconds(m_Waves[m_WaveIndex].DelayBetweenSpawn);
                    }
                }
                else
                {
                    Debug.LogWarning($"Invalid pathIndex in {name}");
                }
            }

            yield return new WaitForSeconds(m_Waves[m_WaveIndex].DelayAfterWaveSpawned);

            m_WaveIndex++;

            if (m_WaveIndex < m_Waves.Length)
            {
                m_Waves[m_WaveIndex].Prepare(StartSpawnEnemies);
                //Включение кнопки форсирования след. волны
                m_EventOnEndSpawnEnemies?.Invoke();
            }
        }
    }
}
