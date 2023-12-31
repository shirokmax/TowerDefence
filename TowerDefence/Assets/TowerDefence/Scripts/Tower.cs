using System.Collections.Generic;
using UnityEngine;
using SpaceShooter;
using static UnityEngine.GraphicsBuffer;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TowerDefence
{
    public class Tower : Destructible
    {
        [Space]
        [SerializeField] private TowerVisualModel m_VisualModel;

        [Space]
        [SerializeField] private Turret m_MainTurret;
        [SerializeField] private float m_Radius;

        [Space]
        [SerializeField] private AIControllerSpawner m_UnitSpawner;
        public AIControllerSpawner UnitSpawner => m_UnitSpawner;

        private Enemy m_Target;

        private void Update()
        {
            if (m_Target == null)
            {
                m_Target = SelectTarget();
            }
            else
            {
                if ((m_Target.transform.position - transform.position).sqrMagnitude > m_Radius * m_Radius)
                {
                    m_Target = null;
                }
                else
                {
                    m_MainTurret.transform.up = m_Target.transform.position - m_MainTurret.transform.position;
                    m_MainTurret.Fire(m_Target);
                }
            }
        }

        private Enemy SelectTarget()
        {
            Enemy potentialTarget = null;

            Collider2D[] enterColliders = Physics2D.OverlapCircleAll(transform.position, m_Radius);

            float minDist = float.MaxValue;

            foreach (var collider in enterColliders)
            {
                if (collider.transform.root.TryGetComponent(out Enemy enemy))
                {
                    float dist = Vector2.Distance(transform.position, enemy.transform.position);

                    if (dist < minDist)
                    {
                        potentialTarget = enemy;
                        minDist = dist;
                    }
                }
            }

            return potentialTarget;
        }

        public void ApplySettings(TowerSettings settings)
        {
            if (settings == null) return;

            m_Nickname = settings.Nickname;

            m_MainTurret.AssignLoadout(settings.TurretProps);
            m_MainTurret.transform.localPosition = new Vector3(settings.TurretPosition.x, settings.TurretPosition.y, 0);
            m_Radius = settings.Radius;

            transform.position += new Vector3(settings.TowerPositionOffset.x, settings.TowerPositionOffset.y, 0);

            m_UnitSpawner.enabled = settings.SpawnUnits;

            if (settings.SpawnUnits == true)
            {
                m_UnitSpawner.transform.localPosition = new Vector3(settings.UnitSpawnerPosition.x, settings.UnitSpawnerPosition.y, 0);
                m_UnitSpawner.GetComponent<CircleArea>().Radius = settings.UnitSpawnerRadius;

                m_UnitSpawner.SetUnitSettings(settings.UnitSettings);
                m_UnitSpawner.SetSpawnCount(settings.UnitsSpawnCount);
                m_UnitSpawner.SetSpawnCountLimit(settings.UnitsSpawnCount);
                m_UnitSpawner.SetRespawnTime(settings.UnitsRespawnTime);
                m_UnitSpawner.SetUnitsSpawnSFXPrefabs(settings.UnitSpawnSFXPrefabs);
            }

            m_VisualModel.ApplySettings(settings);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, m_Radius);
        }

        [CustomEditor(typeof(Tower))]
        public class TowerInspector : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                TowerSettings settings = EditorGUILayout.ObjectField(null, typeof(TowerSettings), false) as TowerSettings;

                if (settings != null)
                    (target as Tower).ApplySettings(settings);
            }
        }
#endif
    }
}
