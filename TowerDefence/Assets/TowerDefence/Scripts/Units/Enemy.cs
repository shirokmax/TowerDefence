using SpaceShooter;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TowerDefence
{
    public class Enemy : Unit
    {
        [Space]
        [SerializeField] private int m_PlayerDamage = 1;
        [SerializeField] private int m_Gold = 10;
        [SerializeField] private int m_DeathScore = 100;

        public override void ApplySettings(UnitSettings settings)
        {
            base.ApplySettings(settings);

            m_PlayerDamage = settings.PlayerDamage;
            m_Gold = settings.Gold;
            m_DeathScore = settings.DeathScore;
        }

        public void DamagePlayer()
        {
            Player.Instance.RemoveLives(m_PlayerDamage);
        }

        public void AddPlayerStats()
        {
            Player.Instance.AddKill();
            Player.Instance.AddGold(m_Gold);
            Player.Instance.AddScore(m_DeathScore);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Enemy))]
    public class EnemyInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            UnitSettings settings = EditorGUILayout.ObjectField(null, typeof(UnitSettings), false) as UnitSettings;

            if (settings != null)
                (target as Enemy).ApplySettings(settings);
        }
    }
#endif
}
