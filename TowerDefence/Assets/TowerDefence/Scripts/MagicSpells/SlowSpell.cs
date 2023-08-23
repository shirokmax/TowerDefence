using System.Collections;
using UnityEngine;
using SpaceShooter;

namespace TowerDefence
{
    public class SlowSpell : MagicSpell
    {
        [Range(0f, 1f)]
        [SerializeField] private float m_SlowRate;

        public override void Use()
        {
            base.Use();

            StartCoroutine(ApplySpell());
        }

        private void Slow(Enemy enemy)
        {
            enemy.ChangeMoveSpeed(m_SlowRate);
        }

        private IEnumerator ApplySpell()
        {
            m_IsSpellActive = true;
            m_UseSpellButtonImage.sprite = m_ActiveSpellSprite;
            m_UseSpellButton.interactable = false;

            Player.Instance.RemoveMana(m_ManaCost);

            foreach (var enemy in FindObjectsOfType<Enemy>())
                enemy.ChangeMoveSpeed(m_SlowRate);
            EnemyWavesManager.EventOnEnemySpawn.AddListener(Slow);

            yield return new WaitForSeconds(m_Duration);

            foreach (var enemy in FindObjectsOfType<Enemy>())
                enemy.RestoreMoveSpeed();
            EnemyWavesManager.EventOnEnemySpawn.RemoveListener(Slow);

            m_IsSpellActive = false;
            m_UseSpellButtonImage.sprite = m_DefaultSpellSprite;
            m_UseSpellButton.interactable = true;
        }
    }
}
