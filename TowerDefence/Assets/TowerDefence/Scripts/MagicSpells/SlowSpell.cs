using System.Collections;
using UnityEngine;
using SpaceShooter;

namespace TowerDefence
{
    public class SlowSpell : MagicSpell
    {
        [SerializeField] private float m_SlowRate;
        [SerializeField] private ImpactEffect m_SlowEffectVFXPrefab;

        [Space]
        [SerializeField] private UpgradeAsset m_UnlockUpgrade;
        [SerializeField] private UpgradeAsset m_SlowRateUpgrade;
        [SerializeField] private UpgradeAsset m_ManaCostUpgrade;

        private float m_SlowTimer;

        protected override void Awake()
        {
            base.Awake();

            if (m_Properties != null)
                ApplyProperties(m_Properties);
        }

        protected override void Start()
        {
            if (Upgrades.GetUpgradeLevel(m_UnlockUpgrade) == 0)
            {
                m_UseSpellButton.interactable = false;
                m_UseSpellButtonImage.color = m_InactiveColor;
                m_magicSpellNameText.color = m_InactiveColor;
                m_ManaCostPanelImage.color = m_InactiveColor;
                m_ManaImage.color = m_InactiveColor;
                m_CostText.color = m_InactiveColor; 
                m_CostText.text = "?";
                return;
            }

            m_ManaCost -= (int)Upgrades.GetCurrentUpgradeValue(m_ManaCostUpgrade);
            m_SlowRate += Upgrades.GetCurrentUpgradeValue(m_SlowRateUpgrade);

            base.Start();
        }

        private void Update()
        {
            if (m_SlowTimer > 0)
                m_SlowTimer -= Time.deltaTime;
        }

        public override void Use()
        {
            base.Use();

            StartCoroutine(ApplySpell());
        }

        private IEnumerator ApplySpell()
        {
            m_IsSpellActive = true;
            m_SlowTimer = m_Duration;
            m_UseSpellButtonImage.sprite = m_ActiveSpellIconSprite;
            m_UseSpellButton.interactable = false;

            Player.Instance.RemoveMana(m_ManaCost);

            foreach (var enemy in FindObjectsOfType<Enemy>())
                Slow(enemy);

            EnemyWavesManager.EventOnEnemySpawn.AddListener(Slow);

            yield return new WaitForSeconds(m_Duration);

            foreach (var enemy in FindObjectsOfType<Enemy>())
                enemy.RestoreMoveSpeed();
            EnemyWavesManager.EventOnEnemySpawn.RemoveListener(Slow);

            m_IsSpellActive = false;
            m_UseSpellButtonImage.sprite = m_DefaultSpellIconSprite;
            m_UseSpellButton.interactable = true;
        }

        private void Slow(Enemy enemy)
        {
            enemy.ChangeMoveSpeed(m_SlowRate);

            if (m_SlowEffectVFXPrefab != null)
            {
                var vfx = Instantiate(m_SlowEffectVFXPrefab, enemy.VisualModel.ShadowSprite.transform);
                vfx.SetLifeTime(m_SlowTimer);
            }
        }

        public override void ApplyProperties(MagicSpellProperties props)
        {
            base.ApplyProperties(props);

            m_SlowRate = props.SlowRate;
        }
    }
}
