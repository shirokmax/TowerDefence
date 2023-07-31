using SpaceShooter;
using UnityEngine;

namespace TowerDefence
{
    public class EnemyWavesManager : MonoSingleton<EnemyWavesManager>
    {
        [SerializeField] private Enemy m_EnemyPrefab;
        [SerializeField] private UnitPath[] m_Paths;

        private EnemyWave[] m_Waves;
        private int m_WaveIndex;

        protected override void Awake()
        {
            base.Awake();

            //Берем волны из всех дочерних объектов
            m_Waves = GetComponentsInChildren<EnemyWave>();
        }

        private void Start()
        {
            if (m_Waves.Length > 0)
                m_Waves[m_WaveIndex].Prepare(SpawnEnemies);
        }

        private void SpawnEnemies()
        {
            foreach ((UnitSettings settings, int count, int pathIndex) in m_Waves[m_WaveIndex].EnumerateSquads())
            {
                if (pathIndex < m_Paths.Length)
                {
                    for (int i = 0; i < count; i++)
                    {
                        Enemy enemy = Instantiate(m_EnemyPrefab, m_Paths[pathIndex].StartArea.GetRandomInsideZone(), Quaternion.identity);
                        enemy.ApplySettings(settings);
                        enemy.GetComponent<AIController>().SetPathBehaviour(m_Paths[pathIndex], AIBehaviour.PathMove);
                    }
                }
                else
                {
                    Debug.LogWarning($"Invalid pathIndex in {name}");
                }
            }

            m_WaveIndex++;

            if (m_WaveIndex < m_Waves.Length)
                m_Waves[m_WaveIndex].Prepare(SpawnEnemies);
        }
    }
}
