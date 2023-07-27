using UnityEngine;

namespace TowerDefence
{
    public abstract class HeroSkill : MonoBehaviour
    {
        [SerializeField] protected string m_SkillName;
        public string Skillname => m_SkillName;

        [SerializeField] protected Sprite m_SkillIconSprite;
        public Sprite SkillIconSprite => m_SkillIconSprite;

        [Min(0f)]
        [SerializeField] protected float m_Cooldown;
        public float Cooldown => m_Cooldown;

        [Space]
        [SerializeField] protected ImpactEffect[] m_ActivationSFXPrefabs;
        [SerializeField] protected ImpactEffect m_ActivationVFXPrefab;
        [SerializeField] protected Transform m_VFXPosition;

        protected Timer m_CooldownTimer;
        public Timer CooldownTimer => m_CooldownTimer;

        protected Hero m_Hero;

        protected virtual void Awake()
        {
            m_Hero = GetComponentInParent<Hero>();

            m_CooldownTimer = new Timer(m_Cooldown);
            m_CooldownTimer.Reset();
        }

        protected virtual void Update()
        {
            m_CooldownTimer.RemoveTime(Time.deltaTime);
        }

        public virtual void OnSkillActivation()
        {
            if (m_ActivationVFXPrefab != null)
            {
                var effect = Instantiate(m_ActivationVFXPrefab, m_VFXPosition.position, Quaternion.identity);
                effect.transform.SetParent(m_VFXPosition);
            }

            if (m_ActivationSFXPrefabs.Length > 0)
            {
                int index = Random.Range(0, m_ActivationSFXPrefabs.Length);
                Instantiate(m_ActivationSFXPrefabs[index]);
            }

            Activation();

            m_CooldownTimer.Restart();
        }

        protected abstract void Activation();
    }
}
