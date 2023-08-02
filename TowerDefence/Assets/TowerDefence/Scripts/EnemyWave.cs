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

        public float PrepareRemainingTime => m_PrepareTime - Time.time;

        private UnityEvent m_EventOnWaveReady = new UnityEvent();

        private static UnityEvent<float> m_EventOnWavePrepare;
        public static UnityEvent<float> EventOnWavePrepare => m_EventOnWavePrepare;

        private void Awake()
        {
            m_EventOnWavePrepare ??= new UnityEvent<float>();

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
            EventOnWavePrepare?.Invoke(m_PrepareTime);
            m_PrepareTime += Time.time;
            m_EventOnWaveReady.AddListener(spawnEnemies);
        }

        public void CancelPrepare()
        {
            enabled = false;
            m_EventOnWaveReady.RemoveAllListeners();
        }

        public IEnumerable<(UnitSettings settings, int count, int pathIndex)> EnumerateSquads()
        {
            // ������ ������ ������ ����� ������� ���� ���� ������ � ������� ����� ��������� ����
            for (int i = 0; i < m_Groups.Length; i++)
            {
                foreach (var squad in m_Groups[i].squads)
                {
                    yield return (squad.settings, squad.count, i);
                }
            }
        }

        public float GetWaveTime()
        {
            int unitsCount = 0;

            foreach(var group in m_Groups)
            {
                foreach (var squad in group.squads)
                    unitsCount += squad.count;
            }

            return m_PrepareTime + unitsCount * m_DelayBetweenSpawn;
        }
    }
}
