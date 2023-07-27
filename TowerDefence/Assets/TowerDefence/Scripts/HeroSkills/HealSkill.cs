using UnityEngine;

namespace TowerDefence
{
    public class HealSkill : HeroSkill
    {
        [Space]
        [SerializeField] private int m_HealAmount = 500;
        public int HealAmount => m_HealAmount;

        protected override void Activation()
        {
            m_Hero.Heal(m_HealAmount);
        }
    }
}
