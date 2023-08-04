using SpaceShooter;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class UnitVisualModel : MonoBehaviour
    {
        #region Properties
        [SerializeField] private Unit m_Unit;

        [Space]
        [SerializeField] private SpriteRenderer m_UnitSprite;
        [SerializeField] private SpriteRenderer m_ShadowSprite;

        [Space]
        [SerializeField] private Animator m_UnitAnimator;
        public Animator UnitAnimator => m_UnitAnimator;

        [Space]
        [SerializeField] private RectTransform m_HPBarRect;
        [SerializeField] private Image m_HPBarImage;

        [Space]
        [SerializeField] private ImpactEffect[] m_TakeDamageVFXPrefabs;
        public ImpactEffect[] TakeDamageVFXPrefabs => m_TakeDamageVFXPrefabs;

        [SerializeField] private Vector2 m_TakeDamageVFXPrefabScale;
        public Vector2 TakeDamageVFXPrefabScale => m_TakeDamageVFXPrefabScale;

        [SerializeField] private CircleArea m_TakeDamageVFXArea;

        [SerializeField] private Vector2 m_TakeDamageVFXAreaPosition;
        public Vector2 TakeDamageVFXAreaPosition => m_TakeDamageVFXAreaPosition;

        [Space]
        [SerializeField] private ImpactEffect[] m_DeathSFXPrefabs;

        [SerializeField] private ImpactEffect[] m_MeleeAttackSFXPrefabs;
        public ImpactEffect[] MeleeAttackSFXPrefabs => m_MeleeAttackSFXPrefabs;

        [SerializeField] private ImpactEffect[] m_AttackVoiceSFXPrefabs;
        public ImpactEffect[] AttackVoiceSFXPrefabs => m_AttackVoiceSFXPrefabs;

        [SerializeField] private ImpactEffect[] m_ConfirmationVoiceSFXPrefabs;
        public ImpactEffect[] ConfirmationVoiceSFXPrefabs => m_ConfirmationVoiceSFXPrefabs;

        [SerializeField] private float m_ConfirmationVoiceCooldown;
        public float ConfirmationVoiceCooldown => m_ConfirmationVoiceCooldown;

        [SerializeField] private ImpactEffect[] m_RespawnVoiceSFXPrefabs;
        public ImpactEffect[] RespawnVoiceSFXPrefabs => m_RespawnVoiceSFXPrefabs;

        [SerializeField] [Range(0.0f, 1.0f)] private float m_MeleeAttackSFXRate = 0.5f;
        public float MeleeAttackSFXRate => m_MeleeAttackSFXRate;

        [SerializeField] [Range(0.0f, 1.0f)] private float m_AttackVoiceRate = 0.7f;
        public float AttackVoiceRate => m_AttackVoiceRate;

        private Vector3 m_ShadowSpriteLocalPos;

        private const float FLIP_DIR_THRESHOLD = 0.05f;

        #endregion

        private void Awake()
        {
            m_ShadowSpriteLocalPos = m_ShadowSprite.transform.localPosition;
        }

        private void Start()
        {
            m_Unit.EventChangeHitPoints.AddListener(HPBarUpdate);
            m_Unit.EventOnDamageTaken.AddListener(SpawnTakeDamageVFX);

            m_UnitAnimator.SetFloat("AttackAnimationSpeed", m_Unit.AttackAnimationSpeed);
        }

        private void Update()
        {
            AnimationsControl();
        }

        private void LateUpdate()
        {
            //ѕоворот спрайта дл€ вертикального положени€
            transform.up = Vector2.up;

            //ѕравильное отображение спрайтов поверх друг-друга
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);

            FlipSprite();
        }

        private void FlipSprite()
        {
            if (CheckAnimationParameter(m_UnitAnimator, "Death") == true && m_UnitAnimator.GetBool("Death") == true)
                return;

            if (transform.root.eulerAngles.z >= 0 && transform.root.eulerAngles.z < 180)
            {
                m_UnitSprite.flipX = true;
                m_ShadowSprite.transform.localPosition = new Vector3(-m_ShadowSpriteLocalPos.x, m_ShadowSpriteLocalPos.y, 0);
            }
            else
            {
                m_UnitSprite.flipX = false;
                m_ShadowSprite.transform.localPosition = new Vector3(m_ShadowSpriteLocalPos.x, m_ShadowSpriteLocalPos.y, 0);
            }
        }

        public void SetAttackVoiceRate(float rate)
        {
            if (rate <= 0f) 
                m_AttackVoiceRate = 0f;
            else if (rate >= 1f) 
                m_AttackVoiceRate = 1f;
            else
                m_AttackVoiceRate = rate;
        }

        public void SetAttackAnimationSpeed(float speed)
        {
            if (speed < 0) return;

            m_UnitAnimator.SetFloat("AttackAnimationSpeed", speed);
        }

        private void AnimationsControl()
        {
            if (CheckAnimationParameter(m_UnitAnimator, "Death") == false || m_UnitAnimator.GetBool("Death") == false)
            {
                if (CheckAnimationParameter(m_UnitAnimator,"Move") == true)
                {
                    if (m_Unit.SpeedControl > FLIP_DIR_THRESHOLD)
                        m_UnitAnimator.SetBool("Move", true);
                    else
                        m_UnitAnimator.SetBool("Move", false);
                }
            }
        }

        public static bool CheckAnimationParameter(Animator animator, string paramName)
        {
            foreach(var par in animator.parameters)
            {
                if (par.name == paramName)
                    return true;
            }

            return false;
        }

        private void HPBarUpdate()
        {
            float HPNormalized = (float)m_Unit.CurrentHitPoints / m_Unit.MaxHitPoints;
            m_HPBarImage.fillAmount = HPNormalized;
        }

        private void SpawnTakeDamageVFX()
        {
            if (m_TakeDamageVFXPrefabs.Length > 0)
            {
                int index = Random.Range(0, m_TakeDamageVFXPrefabs.Length);

                var effect = Instantiate(m_TakeDamageVFXPrefabs[index]);
                effect.transform.position = m_TakeDamageVFXArea.GetRandomInsideZone();
                effect.transform.localScale = new Vector3(TakeDamageVFXPrefabScale.x, TakeDamageVFXPrefabScale.y, 1);
                effect.transform.SetParent(transform);
            }
        }

        public void ApplyUnitSettings(UnitSettings settings)
        {
            m_UnitAnimator.runtimeAnimatorController = settings.Animations;

            m_TakeDamageVFXPrefabs = settings.TakeDamageVFXPrefabs;
            m_TakeDamageVFXPrefabScale = settings.TakeDamageVFXPrefabScale;

            m_TakeDamageVFXArea.Radius = settings.TakeDamageVFXAreaRadius;
            m_TakeDamageVFXArea.transform.localPosition = new Vector3(settings.TakeDamageVFXAreaPosition.x, settings.TakeDamageVFXAreaPosition.y, 0);

            m_MeleeAttackSFXPrefabs = settings.MeleeAttackSFXPrefabs;
            m_AttackVoiceSFXPrefabs = settings.AttackVoiceSFXPrefabs;
            m_ConfirmationVoiceSFXPrefabs = settings.ConfirmationVoiceSFXPrefabs;
            m_ConfirmationVoiceCooldown = settings.ConfirmationVoiceCooldown;
            m_RespawnVoiceSFXPrefabs = settings.RespawnVoiceSFXPrefabs;

            m_MeleeAttackSFXRate = settings.MeleeAttackSFXRate;
            m_AttackVoiceRate = settings.AttackVoiceRate;

            m_UnitSprite.sprite = settings.UnitSprite;
            m_UnitSprite.color = settings.UnitSpriteColor;
            m_UnitSprite.transform.localScale = new Vector3(settings.UnitSpriteScale.x, settings.UnitSpriteScale.y, 1);
            m_UnitSprite.transform.localPosition = new Vector3(settings.UnitSpritePosition.x, settings.UnitSpritePosition.y, 0);

            m_ShadowSprite.transform.localScale = new Vector3(settings.ShadowSpriteScale.x, settings.ShadowSpriteScale.y, 1);
            m_ShadowSprite.transform.localPosition = new Vector3(settings.ShadowPosition.x, settings.ShadowPosition.y, 0);
            m_ShadowSpriteLocalPos = m_ShadowSprite.transform.localPosition;

            m_HPBarRect.sizeDelta = settings.HPBarSize;
            m_HPBarRect.anchoredPosition = settings.HPBarPosition;

            m_DeathSFXPrefabs = settings.DeathSFXPrefabs;
        }

        public void InstantiateRandomDeathSound()
        {
            if (m_DeathSFXPrefabs.Length > 0)
            {
                int index = Random.Range(0, m_DeathSFXPrefabs.Length);
                Instantiate(m_DeathSFXPrefabs[index], transform.root.position, Quaternion.identity);
            }
        }
    }
}
