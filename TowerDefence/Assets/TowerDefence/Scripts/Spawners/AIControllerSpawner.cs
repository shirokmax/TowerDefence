using UnityEngine;
using TowerDefence;
using System.Collections.Generic;

namespace SpaceShooter
{
    public class AIControllerSpawner : Spawner
    {
        [Space]
        [SerializeField] private Unit m_UnitPrefab;
        [SerializeField] private UnitSettings[] m_UnitSettings;

        [Space]
        [SerializeField] private Team m_TeamId;

        [Space]
        [SerializeField] AIBehaviour m_AIBehaviour;

        [Space]
        [SerializeField] private CircleArea m_PatrolArea;
        [SerializeField] private UnitPath m_Path;
        [SerializeField] private Transform m_UnitsHoldPoint;

        [Space]
        [SerializeField] private int m_AdditionalUnitsHitPoints;
        [SerializeField] private int m_AdditionalUnitsDamage;

        [Space]
        [SerializeField] private ImpactEffect[] m_UnitSpawnSFXPrefabs;

        private Transform[] m_HoldPoints;
        private List<Unit> m_CurrentSpawnedUnits;

        private bool m_SpawnSFXPlayed;

        protected override void Awake()
        {
            base.Awake();

            m_CurrentSpawnedUnits = new List<Unit>();

            if (m_AIBehaviour == AIBehaviour.HoldPosition)
            {
                m_HoldPoints = new Transform[m_UnitsHoldPoint.childCount];
                for (int i = 0; i < m_UnitsHoldPoint.childCount; i++)
                    m_HoldPoints[i] = m_UnitsHoldPoint.GetChild(i);
            }
        }

        protected override void UpdateTimer()
        {
            if (m_CurrentSpawnedCount < m_SpawnCountLimit)
                base.UpdateTimer();
        }

        protected override void Spawn()
        {
            if (m_UnitSettings.Length == 0) return;

            int holdPointIndex = 0;
            m_SpawnSFXPlayed = false;

            for (int i = 0; i < m_SpawnCount; i++)
            {
                if (m_SpawnCountLimit == 0 || m_CurrentSpawnedCount < m_SpawnCountLimit)
                {
                    Unit unit = Instantiate(m_UnitPrefab);
                    unit.transform.position = m_SpawnArea.GetRandomInsideZone();

                    int settingsIndex = Random.Range(0, m_UnitSettings.Length);
                    unit.ApplySettings(m_UnitSettings[settingsIndex]);

                    unit.SetMaxHitPoints(unit.MaxHitPoints + m_AdditionalUnitsHitPoints);
                    unit.SetMeleeDamage(unit.MeleeDamage + m_AdditionalUnitsDamage);

                    var dest = unit.GetComponent<Destructible>();

                    dest.SetTeamId(m_TeamId);
                    dest.EventOnDestroy.AddListener(OnDestroyEntity);
                    dest.EventOnDestroy.AddListener(() => m_CurrentSpawnedUnits.Remove(unit));

                    AIController unitAI = unit.GetComponent<AIController>();

                    switch (m_AIBehaviour)
                    {
                        case AIBehaviour.None:
                            unitAI.SetNoneBehaviour();
                            break;

                        case AIBehaviour.HoldPosition:
                            {
                                unitAI.SetHoldPositionBehaviour(m_HoldPoints[holdPointIndex]);
                                holdPointIndex++;
                            }
                            break;

                        case AIBehaviour.AreaPatrol:
                            unitAI.SetAreaPatrolBehaviour(m_PatrolArea);
                            break;

                        case AIBehaviour.PathPatrol:
                            unitAI.SetPathBehaviour(m_Path, AIBehaviour.PathPatrol);
                            break;

                        case AIBehaviour.PathMove:
                            unitAI.SetPathBehaviour(m_Path, AIBehaviour.PathMove);
                            break;

                        default:
                            unitAI.SetNoneBehaviour();
                            break;
                    }

                    m_CurrentSpawnedCount++;
                    m_CurrentSpawnedUnits.Add(unit);
                    m_Timer = m_RespawnTime;

                    if (m_UnitSpawnSFXPrefabs.Length > 0 && m_SpawnSFXPlayed == false)
                    {
                        int index = Random.Range(0, m_UnitSpawnSFXPrefabs.Length);
                        Instantiate(m_UnitSpawnSFXPrefabs[index]);
                        m_SpawnSFXPlayed = true;
                    }
                }
            }
        }

        private void OnDestroy()
        {
            foreach (Unit unit in m_CurrentSpawnedUnits)
                Destroy(unit.gameObject);
        }

        #region Set Parameters
        public void SetUnitSettings(UnitSettings[] settings)
        {
            if (settings == null || settings.Length <= 0) return;

            m_UnitSettings = settings;
        }

        public void SetUnitSettings(UnitSettings settings)
        {
            if (settings == null) return;

            m_UnitSettings = new UnitSettings[1] { settings };
        }

        public void SetSpawnCount(int count)
        {
            if (count <= 0) return;

            m_SpawnCount = count;
        }

        public void SetRespawnTime(float time)
        {
            if (time < 0)
                m_RespawnTime = 0;

            m_RespawnTime = time;
        }

        public void SetSpawnCountLimit(int limit)
        {
            if (limit < 0)
                m_SpawnCountLimit = 0;

            m_SpawnCountLimit = limit;
        }

        public void SetUnitsHoldPointPosition(Transform point)
        {
            if (point == null) return;

            m_UnitsHoldPoint.position = point.position;
        }

        public void SetUnitsSpawnSFXPrefabs(ImpactEffect[] effect)
        {
            if (effect == null || effect.Length <= 0) return;

            m_UnitSpawnSFXPrefabs = effect;
        }

        public void SetAdditionalUnitsHitPoints(int hp)
        {
            m_AdditionalUnitsHitPoints += hp;
        }

        public void SetAdditionalUnitsDamage(int damage)
        {
            m_AdditionalUnitsDamage += damage;
        }

        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (m_AIBehaviour == AIBehaviour.AreaPatrol && m_PatrolArea != null)
            {
                // Радиус зоны патруля
                UnityEditor.Handles.color = new Color(1, 1, 0, 0.02f);
                UnityEditor.Handles.DrawSolidDisc(m_PatrolArea.transform.position, Vector3.forward, m_PatrolArea.Radius);
            }
        }
#endif
    }
}
