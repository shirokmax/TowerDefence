using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter;

namespace TowerDefence
{
    public class LevelConditionWaves : MonoBehaviour, ILevelCondition
    {
        private bool m_Reached;

        private void Start()
        {
            EnemyWavesManager.Instance.EventOnAllWavesDead.AddListener(() => m_Reached = true);
        }

        public LevelCondition Condition => LevelCondition.Waves;

        public string Description => "Complete all waves";

        public bool IsCompleted => m_Reached;
    }
}
