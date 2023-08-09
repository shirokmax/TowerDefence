using UnityEngine;

namespace TowerDefence
{
    public class UnitAnimationEvents : MonoBehaviour
    {
        [SerializeField] private Unit m_Unit;
        [SerializeField] private UnitVisualModel m_VisualModel;

        private Unit m_TargetUnit;

        public void SetTargetUnit(Unit unit)
        {
            m_TargetUnit = unit;
        }

        public void OnDeathAnimationEnd()
        {
            Destroy(transform.root.gameObject);
        }

        public void AttackAnimationDamage()
        {
            if (m_TargetUnit != null)
                m_TargetUnit.TakeDamage(m_Unit.MeleeDamage);
        }

        public void AttackAnimationSound()
        {
            if (m_VisualModel.MeleeAttackSFXPrefabs.Length > 0)
            {
                if (Random.value <= m_VisualModel.MeleeAttackSFXRate)
                {
                    int index = Random.Range(0, m_VisualModel.MeleeAttackSFXPrefabs.Length);
                    Instantiate(m_VisualModel.MeleeAttackSFXPrefabs[index]);
                }
            }
        }

        public void AttackAnimationVoice()
        {
            if (m_VisualModel.AttackVoiceSFXPrefabs.Length > 0)
            {
                if (Random.value <= m_VisualModel.AttackVoiceRate)
                {
                    int index = Random.Range(0, m_VisualModel.AttackVoiceSFXPrefabs.Length);
                    Instantiate(m_VisualModel.AttackVoiceSFXPrefabs[index]);
                }
            }
        }
    }
}
