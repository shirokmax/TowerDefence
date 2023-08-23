using UnityEngine;
using UnityEngine.UI;
using SpaceShooter;
using System.Collections;

namespace TowerDefence
{
    public class FireSpell : MagicSpell
    {
        [SerializeField] private int m_DamagePerSecond = 200;
        [SerializeField] private int m_DamageTicksPerSecond = 4;
        [SerializeField] private float m_Radius = 0.8f;
        [SerializeField] private Image m_TargetCircle;

        [Space]
        [SerializeField] private ImpactEffect m_FireEffectVFXPrefab;
        [SerializeField] private ImpactEffect m_FireEffectSFXPrefab;

        private bool m_IsTargeting;

        private void Update()
        {
            if (m_IsTargeting == true)
                m_TargetCircle.transform.position = Input.mousePosition;
        }

        public override void Use()
        {
            base.Use();

            m_IsTargeting = true;
            m_UseSpellButtonImage.sprite = m_ActiveSpellSprite;
            m_TargetCircle.enabled = true;

            UIClickProtection.Instance.Activate(Fire);
        }

        private void Fire(Vector2 targetPos, bool confirmation)
        {
            if (confirmation == true)
            {
                StartCoroutine(FireBurn(targetPos));
            }
            else
            {
                m_IsTargeting = false;
                m_TargetCircle.enabled = false;
                m_UseSpellButtonImage.sprite = m_DefaultSpellSprite;
            }
        }

        private IEnumerator FireBurn(Vector2 targetPos)
        {
            m_IsTargeting = false;
            m_TargetCircle.enabled = false;

            m_IsSpellActive = true;
            Player.Instance.RemoveMana(m_ManaCost);

            Vector3 position = targetPos;
            position.z = Camera.main.nearClipPlane;
            position = Camera.main.ScreenToWorldPoint(position);

            if (m_FireEffectVFXPrefab != null)
            {
                var vfx = Instantiate(m_FireEffectVFXPrefab, position, Quaternion.identity);
                vfx.SetLifeTime(m_Duration);
            }

            if (m_FireEffectSFXPrefab != null)
            {
                var sfx = Instantiate(m_FireEffectSFXPrefab);
                sfx.SetLifeTime(m_Duration);
            }

            float timer = m_Duration;
            float timePerTick = 1f / m_DamageTicksPerSecond;

            while (timer > 0)
            {
                foreach (var collider in Physics2D.OverlapCircleAll(position, m_Radius))
                {
                    if (collider.transform.root.TryGetComponent(out Enemy enemy))
                        enemy.TakeDamage(m_DamagePerSecond / m_DamageTicksPerSecond, DamageType.Pure);
                }

                timer -= timePerTick;
                yield return new WaitForSeconds(1f / m_DamageTicksPerSecond);
            }

            m_IsSpellActive = false;
            m_UseSpellButtonImage.sprite = m_DefaultSpellSprite;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = new Color(1, 0, 0, 0.3f);
            UnityEditor.Handles.DrawSolidDisc(Vector3.zero, Vector3.forward, m_Radius);
        }
#endif
    }
}
