using UnityEngine;
using SpaceShooter;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TowerDefence
{
    public enum ArmorType
    {
        Physical,
        Magic
    }

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

        [Space]
        [SerializeField] private MovementType m_MoveType;
        public MovementType MoveType => m_MoveType;

        [SerializeField] private float m_MoveSpeed;
        public float MoveSpeed => m_MoveSpeed;

        [Space]
        [SerializeField] private ArmorType m_ArmorType;
        public ArmorType ArmorType => m_ArmorType;

        [SerializeField] private int m_Armor;
        public int Armor => m_Armor;

        [Space]
        [SerializeField] private DamageType m_DamageType;
        public DamageType DamageType => m_DamageType;

        [SerializeField] private int m_MeleeDamage;
        public int MeleeDamage => m_MeleeDamage;

        //Изменения скорости анимации видно только в аниматоре
        [SerializeField] private float m_AttackAnimationSpeed = 1f;
        public float AttackAnimationSpeed => m_AttackAnimationSpeed;

        [SerializeField] private float m_MeleeAttackRangeRadius;
        public float MeleeAttackRangeRadius => m_MeleeAttackRangeRadius;

        /// <summary>
        /// Управление скоростью передвижения. (от -1.0 до 1.0)
        /// </summary>
        public float SpeedControl { get; set; }

        private static HashSet<Unit> m_AllUnits;
        public static IReadOnlyCollection<Unit> AllUnits => m_AllUnits;

        private static Func<int, DamageType, int, int>[] ArmorDamageFunctions =
{
            // ArmorType.Physical
            (int damage, DamageType dmgType, int armor) =>
            {
                switch (dmgType)
                {
                    case DamageType.Magic: return damage;
                    default: return Mathf.Max(1, damage - armor);
                }
            },
            // ArmorType.Magic
            (int damage, DamageType dmgType, int armor) =>
            {
                switch (dmgType)
                {
                    case DamageType.Magic: return Mathf.Max(1, damage - armor);
                    default: return Mathf.Max(1, damage - armor / 2);
                }
            }
        };

        #endregion

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            if (SpeedControl != 0)
                transform.Translate(SpeedControl * m_MoveSpeed * Vector3.up * Time.deltaTime);
        }

        public virtual void ApplySettings(UnitSettings settings)
        {
            if (settings == null) return;

            m_Nickname = settings.UnitName;

            m_MaxHitPoints = settings.HitPoints;
            m_ArmorType = settings.ArmorType;
            m_Armor = settings.Armor;
            m_MoveType = settings.MoveType;
            m_MoveSpeed = settings.MoveSpeed;
            m_DamageType = settings.DamageType;
            m_MeleeDamage = settings.MeleeDamage;
            m_AttackAnimationSpeed = settings.AttackAnimationSpeed;
            m_MeleeAttackRangeRadius = settings.MeleeAttackRangeRadius;

            m_Collider.radius = settings.ColliderRadius;
            m_Collider.transform.localPosition = new Vector3(settings.ColliderPosition.x, settings.ColliderPosition.y, 0);

            m_VisualModel.ApplyUnitSettings(settings);
            m_AIController.ApplyUnitSettings(settings);
        }

        public void TakeDamage(int damage, DamageType dmgType)
        {
            ApplyDamage(ArmorDamageFunctions[(int)m_ArmorType](damage, dmgType, m_Armor));
        }

        public void TakeDamage(Destructible fromDest, int damage, DamageType dmgType)
        {
            ApplyDamage(fromDest, ArmorDamageFunctions[(int)m_ArmorType](damage, dmgType, m_Armor));
        }

        protected override void OnDeath()
        {
            // Проигрывание анимации смерти, если таковая есть.
            // После вызова ивента смерти должна срабатывать функция запуска анимации смерти с анимационным ивентом (её нужно назначать в инспекторе), который уже вызывает уничтожение объекта.
            if (UnitVisualModel.CheckAnimationParameter(m_VisualModel.UnitAnimator, "Death") == true)
            {
                m_VisualModel.UnitAnimator.SetBool("Death", true);

                m_EventOnDeath?.Invoke();

                // Уничтожение всех эффектов, которые висели при смерти юнита
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

        public void SetMaxHitPoints(int hp)
        {
            if (hp <= 0) return;

            m_MaxHitPoints = hp;
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

        private void OnDisable()
        {
            SpeedControl = 0;
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
