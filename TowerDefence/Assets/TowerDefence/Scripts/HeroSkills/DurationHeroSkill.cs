using UnityEngine;

namespace TowerDefence
{
    public abstract class DurationHeroSkill : HeroSkill
    {
        [Space]
        [SerializeField] protected float m_DurationTime;
        public float DurationTime => m_DurationTime;

        [SerializeField] protected ImpactEffect m_DurationVFXPrefab;

        protected Timer m_DurationTimer;
        public Timer DurationTimer => m_DurationTimer;

        protected bool m_DurationTimeEnd = true;

        protected override void Awake()
        {
            base.Awake();

            m_DurationTimer = new Timer(m_DurationTime);
            m_DurationTimer.Reset();
        }

        protected override void Update()
        {
            base.Update();

            m_DurationTimer.RemoveTime(Time.deltaTime);

            if (m_DurationTimer.IsFinished == true && m_DurationTimeEnd == false)
            {
                m_DurationTimeEnd = true;

                OnDurationTimeEnd();
            }
        }

        protected abstract void OnDurationTimeEnd();

        public override void OnSkillActivation()
        {
            base.OnSkillActivation();

            if (m_DurationVFXPrefab != null)
            {
                var effect = Instantiate(m_DurationVFXPrefab, m_VFXPosition.position, Quaternion.identity);
                effect.transform.SetParent(m_VFXPosition);
                effect.SetLifeTime(m_DurationTime);
            }

            m_DurationTimeEnd = false;
            m_DurationTimer.Restart();
        }
    }
}
