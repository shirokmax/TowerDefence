using UnityEngine;
using SpaceShooter;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TowerDefence
{
    public enum MovementType
    {
        Walking,
        Flying
    }

    [RequireComponent(typeof(AIController))]
    public class Unit : Destructible
    {
        #region Properties
        [Space]
        [SerializeField] private AIController m_AIController;
        public AIController AIController => m_AIController;

        [SerializeField] private CircleCollider2D m_Collider;
        public CircleCollider2D Collider => m_Collider;

        [SerializeField] private UnitVisualModel m_VisualModel;
        public UnitVisualModel VisualModel => m_VisualModel;

        [SerializeField] private UnitAnimationEvents m_AnimationEvents;
        public UnitAnimationEvents AnimationEvents => m_AnimationEvents;

        [SerializeField] private MovementType m_MoveType;
        public MovementType MoveType => m_MoveType;

        [SerializeField] private float m_MoveSpeed;
        public float MoveSpeed => m_MoveSpeed;

        [SerializeField] private int m_MeleeDamage;
        public int MeleeDamage => m_MeleeDamage;

        //»зменени€ скорости анимации видно только в аниматоре
        [SerializeField] private float m_AttackAnimationSpeed = 1f;
        public float AttackAnimationSpeed => m_AttackAnimationSpeed;

        [SerializeField] private float m_MeleeAttackRangeRadius;
        public float MeleeAttackRangeRadius => m_MeleeAttackRangeRadius;

        //[Space]
        //[SerializeField] private bool m_CanUsePowerups;
        //public bool CanUsePowerups => m_CanUsePowerups;

        private float m_SpeedBoostMult;
        private float m_SpeedBoostTimer;
        public float SpeedBoostTimer => m_SpeedBoostTimer;

        private float m_InvincibleTimer;
        public float InvincibleTimer => m_InvincibleTimer;

        public float SpeedBoostLastDurationTime { get; private set; }
        public float InvincibleLastDurationTime { get; private set; }

        [HideInInspector] public bool isInvincibleWasOn;
        [HideInInspector] public bool isSpeedBoostWasOn;

        public bool IsSpeedBoostActive => m_SpeedBoostTimer > 0;
        public bool IsInvincibleActive => m_InvincibleTimer > 0;

        /// <summary>
        /// ”правление скоростью передвижени€. (от -1.0 до 1.0)
        /// </summary>
        public float SpeedControl { get; set; }

        private static HashSet<Unit> m_AllUnits;
        public static IReadOnlyCollection<Unit> AllUnits => m_AllUnits;

        #endregion

        protected virtual void Awake()
        {
            m_SpeedBoostMult = 1;
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            if (SpeedControl != 0)
                transform.Translate(SpeedControl * m_SpeedBoostMult * m_MoveSpeed * Vector3.up * Time.deltaTime);
        }

        public virtual void ApplySettings(UnitSettings settings)
        {
            if (settings == null) return;

            m_Nickname = settings.UnitName;
            m_MoveType = settings.MoveType;

            m_MaxHitPoints = settings.HitPoints;
            m_MoveSpeed = settings.MoveSpeed;
            m_MeleeDamage = settings.MeleeDamage;
            m_AttackAnimationSpeed = settings.AttackAnimationSpeed;
            m_MeleeAttackRangeRadius = settings.MeleeAttackRangeRadius;

            m_Collider.radius = settings.ColliderRadius;
            m_Collider.transform.localPosition = new Vector3(settings.ColliderPosition.x, settings.ColliderPosition.y, 0);

            m_VisualModel.ApplyUnitSettings(settings);
            m_AIController.ApplyUnitSettings(settings);
        }

        protected override void OnDeath()
        {
            // ѕроигрывание анимации смерти, если такова€ есть.
            // ѕосле вызова ивента смерти должна срабатывать функци€ запуска анимации смерти с анимационным ивентом (еЄ нужно назначать в инспекторе), который уже вызывает уничтожение объекта.
            if (UnitVisualModel.CheckAnimationParameter(m_VisualModel.UnitAnimator, "Death") == true)
            {
                m_VisualModel.UnitAnimator.SetBool("Death", true);

                m_EventOnDeath?.Invoke();

                // ”ничтожение всех эффектов, которые висели при смерти юнита
                var effects = m_VisualModel.GetComponentsInChildren<ImpactEffect>();
                for (int i = 0; i < effects.Length; i++)
                    Destroy(effects[i].gameObject);

                m_VisualModel.transform.parent = null;
                Destroy(m_Collider);
                Destroy(gameObject);
            }
            else
            {
                base.OnDeath();
            }
        }

        public void SetMeleeDamage(int damage)
        {
            if (damage < 0) return;

            m_MeleeDamage = damage;
        }

        public void SetMoveSpeed(float speed)
        {
            if (speed < 0) return;

            m_MoveSpeed = speed;
        }

        #region Powerups
        private void CheckInvincible()
        {
            //if (IsInvincibleActive == true)
            //    m_InvincibleTimer -= Time.fixedDeltaTime;

            //if (IsInvincibleActive == false)
            //    InvincibleOff();
        }

        private void CheckSpeedBoost()
        {
            //if (IsSpeedBoostActive == true)
            //    m_SpeedBoostTimer -= Time.fixedDeltaTime;

            //if (IsSpeedBoostActive == false)
            //    m_SpeedBoostMult = 1;
        }

        public void SpeedBoostOn(float boostMult, float boostTime)
        {
            //isSpeedBoostWasOn = true;

            //m_SpeedBoostMult = boostMult;
            //m_SpeedBoostTimer = boostTime;

            //SpeedBoostLastDurationTime = boostTime;
        }

        /// <summary>
        /// ¬ключает неу€звимость.
        /// </summary>
        public void InvincibleOn()
        {
            //isInvincibleWasOn = true;

            //if (m_Indestructible == false)
            //    m_Indestructible = true;
        }

        /// <summary>
        /// ¬ключает неу€звимость по таймеру.
        /// </summary>
        /// <param name="time">¬рем€ неу€звимости.</param>
        public void InvincibleOn(float time)
        {
            //InvincibleOn();
            //m_InvincibleTimer = time;

            //InvincibleLastDurationTime = time;
        }

        /// <summary>
        /// ¬ыключает неу€звимость.
        /// </summary>
        public void InvincibleOff()
        {
            //if (m_Indestructible == true)
            //    m_Indestructible = false;
        }

        #endregion

        public void Fire()
        {
            return;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (m_AllUnits == null)
                m_AllUnits = new HashSet<Unit>();

            m_AllUnits.Add(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            m_AllUnits.Remove(this);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Handles.color = Color.blue;
            Handles.DrawWireDisc(transform.position, Vector3.forward, m_MeleeAttackRangeRadius);
        }
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Unit))]
    public class UnitInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            UnitSettings settings = EditorGUILayout.ObjectField(null, typeof(UnitSettings), false) as UnitSettings;

            if (settings != null)
                (target as Unit).ApplySettings(settings);
        }
    }
#endif
}
