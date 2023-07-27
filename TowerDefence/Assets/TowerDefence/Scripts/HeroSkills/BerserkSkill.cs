using UnityEngine;

namespace TowerDefence
{
    public class BerserkSkill : DurationHeroSkill
    {
        [Space]
        [SerializeField] private float m_MoveSpeedIncreaseRate = 0.5f;
        public float MoveSpeedIncreaseRate => m_MoveSpeedIncreaseRate;

        [SerializeField] private float m_AttackSpeedIncreaseRate = 1f;
        public float AttackSpeedIncreaseRate => m_AttackSpeedIncreaseRate;

        [SerializeField] private float m_DamageIncreaseRate = 0.5f;
        public float DamageIncreaseRate => m_DamageIncreaseRate;

        private float m_StartMoveSpeed;
        private int m_StartMeleeDamage;
        private float m_StartAttackAnimationSpeed;
        private float m_StartAttackVoiceRate;

        private void Start()
        {
            m_StartMoveSpeed = m_Hero.MoveSpeed;
            m_StartMeleeDamage = m_Hero.MeleeDamage;
            m_StartAttackAnimationSpeed = m_Hero.AttackAnimationSpeed;
            m_StartAttackVoiceRate = m_Hero.VisualModel.AttackVoiceRate;
        }

        protected override void Activation()
        {
            m_Hero.SetMoveSpeed(m_StartMoveSpeed + m_Hero.MoveSpeed * m_MoveSpeedIncreaseRate);
            m_Hero.SetMeleeDamage(m_StartMeleeDamage + (int)(m_Hero.MeleeDamage * m_DamageIncreaseRate));
            m_Hero.VisualModel.SetAttackAnimationSpeed(m_StartAttackAnimationSpeed + m_StartAttackAnimationSpeed * m_AttackSpeedIncreaseRate);
            m_Hero.VisualModel.SetAttackVoiceRate(0f);
        }

        protected override void OnDurationTimeEnd()
        {
            m_Hero.SetMoveSpeed(m_StartMoveSpeed);
            m_Hero.SetMeleeDamage(m_StartMeleeDamage);
            m_Hero.VisualModel.SetAttackAnimationSpeed(m_StartAttackAnimationSpeed);
            m_Hero.VisualModel.SetAttackVoiceRate(m_StartAttackVoiceRate);
        }
    }
}
