using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TowerDefence
{
    public class EnemyWave : MonoBehaviour
    {
        [Serializable]
        private class Squad
        {
            public UnitSettings settings;
            public int count;
        }

        [Serializable]
        private class PathGroup
        {
            public Squad[] squads;
        }

        [SerializeField] private PathGroup[] m_Groups;
        [SerializeField] private float m_PrepareTime = 10f;

        [SerializeField] private float m_DelayBetweenSpawn = 1.5f;
        public float DelayBetweenSpawn => m_DelayBetweenSpawn;

        private UnityEvent m_EventOnWaveReady = new UnityEvent();

        private void Awake()
        {
            enabled = false;
        }

        private void Update()
        {
            if (Time.time >= m_PrepareTime)
            {
                enabled = false;
                m_EventOnWaveReady?.Invoke();
            }
        }

        public void Prepare(UnityAction spawnEnemies)
        {
            enabled = true;
            m_PrepareTime += Time.time;
            m_EventOnWaveReady.AddListener(spawnEnemies);
        }

        public IEnumerable<(UnitSettings settings, int count, int pathIndex)> EnumerateSquads()
        {
            // Индекс каждой группы равен индексу пути этой группы в массиве путей менеджера волн
            for (int i = 0; i < m_Groups.Length; i++)
            {
                foreach (var squad in m_Groups[i].squads)
                {
                    yield return (squad.settings, squad.count, i);
                }
            }
        }
    }
}
