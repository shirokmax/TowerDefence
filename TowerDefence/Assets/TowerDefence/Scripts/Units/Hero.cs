using UnityEngine;

namespace TowerDefence
{
    public class Hero : Unit
    {
        [SerializeField] private Sprite m_HeroIconSprite;
        public Sprite HeroIconSprite => m_HeroIconSprite;

        [SerializeField] private float m_RespawnCooldown = 30f;
        public float RespawnCooldown => m_RespawnCooldown;

        private HeroSkill[] m_HeroSkills;
        public HeroSkill[] HeroSkills => m_HeroSkills;

        protected override void Awake()
        {
            base.Awake();

            m_HeroSkills = transform.GetComponentsInChildren<HeroSkill>();
        }
    }
}
