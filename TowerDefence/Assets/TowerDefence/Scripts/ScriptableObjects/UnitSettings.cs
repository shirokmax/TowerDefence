using UnityEngine;
using SpaceShooter;

namespace TowerDefence
{
    [CreateAssetMenu]
    public sealed class UnitSettings : ScriptableObject
    {
        [Header("Внешний вид")]
        [SerializeField] private Sprite m_UnitSprite;
        public Sprite UnitSprite => m_UnitSprite;

        [SerializeField] private Color m_UnitSpriteColor = Color.white;
        public Color UnitSpriteColor => m_UnitSpriteColor;

        [SerializeField] private RuntimeAnimatorController m_Animations;
        public RuntimeAnimatorController Animations => m_Animations;

        [Space]
        [SerializeField] private Vector2 m_UnitSpriteScale = Vector2.one;
        public Vector2 UnitSpriteScale => m_UnitSpriteScale;

        [SerializeField] private Vector2 m_UnitSpritePosition = Vector2.zero;
        public Vector2 UnitSpritePosition => m_UnitSpritePosition;

        [Space]
        [SerializeField] private Vector2 m_ShadowSpriteScale = Vector2.one;
        public Vector2 ShadowSpriteScale => m_ShadowSpriteScale;

        [SerializeField] private Vector2 m_ShadowPosition = Vector2.zero;
        public Vector2 ShadowPosition => m_ShadowPosition;

        [Space]
        [SerializeField] private Vector2 m_HPBarSize = new Vector2(40, 50);
        public Vector2 HPBarSize => m_HPBarSize;

        [SerializeField] private Vector2 m_HPBarPosition = new Vector2(0, 26);
        public Vector2 HPBarPosition => m_HPBarPosition;

        [Space]
        [SerializeField] private ImpactEffect[] m_TakeDamageVFXPrefabs;
        public ImpactEffect[] TakeDamageVFXPrefabs => m_TakeDamageVFXPrefabs;

        [SerializeField] private Vector2 m_TakeDamageVFXPrefabScale = new Vector2(1.2f, 1.2f);
        public Vector2 TakeDamageVFXPrefabScale => m_TakeDamageVFXPrefabScale;

        [SerializeField] private float m_TakeDamageVFXAreaRadius;
        public float TakeDamageVFXAreaRadius => m_TakeDamageVFXAreaRadius;

        [SerializeField] private Vector2 m_TakeDamageVFXAreaPosition;
        public Vector2 TakeDamageVFXAreaPosition => m_TakeDamageVFXAreaPosition;

        [Space]
        [SerializeField] private ImpactEffect[] m_DeathSFXPrefabs;
        public ImpactEffect[] DeathSFXPrefabs => m_DeathSFXPrefabs;

        [SerializeField] private ImpactEffect[] m_MeleeAttackSFXPrefabs;
        public ImpactEffect[] MeleeAttackSFXPrefabs => m_MeleeAttackSFXPrefabs;

        [SerializeField] private ImpactEffect[] m_AttackVoiceSFXPrefabs;
        public ImpactEffect[] AttackVoiceSFXPrefabs => m_AttackVoiceSFXPrefabs;

        [SerializeField] private ImpactEffect[] m_ConfirmationVoiceSFXPrefabs;
        public ImpactEffect[] ConfirmationVoiceSFXPrefabs => m_AttackVoiceSFXPrefabs;

        [SerializeField] private float m_ConfirmationVoiceCooldown;
        public float ConfirmationVoiceCooldown => m_ConfirmationVoiceCooldown;

        [SerializeField] private ImpactEffect[] m_RespawnVoiceSFXPrefabs;
        public ImpactEffect[] RespawnVoiceSFXPrefabs => m_RespawnVoiceSFXPrefabs;

        [Space]
        [SerializeField][Range(0.0f, 1.0f)] private float m_MeleeAttackSFXRate = 0.5f;
        public float MeleeAttackSFXRate => m_MeleeAttackSFXRate;

        [SerializeField][Range(0.0f, 1.0f)] private float m_AttackVoiceRate = 0.7f;
        public float AttackVoiceRate => m_AttackVoiceRate;

        [Header("Игровые параметры")]
        [SerializeField] private string m_UnitName = "Knight";
        public string UnitName => m_UnitName;

        [SerializeField] private MovementType m_MoveType = MovementType.Walking;
        public MovementType MoveType => m_MoveType;

        [SerializeField] private int m_MeleeDamage = 5;
        public int MeleeDamage => m_MeleeDamage;

        [SerializeField] private float m_AttackAnimationSpeed = 1f;
        public float AttackAnimationSpeed => m_AttackAnimationSpeed;

        [SerializeField] private float m_MeleeAttackRangeRadius = 0.4f;
        public float MeleeAttackRangeRadius => m_MeleeAttackRangeRadius;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_SpeedControl = 1f;
        public float SpeedControl => m_SpeedControl;

        [Space]
        [SerializeField] private float m_MoveSpeed = 1;
        public float MoveSpeed => m_MoveSpeed;

        [SerializeField] private int m_HitPoints = 10;
        public int HitPoints => m_HitPoints;

        [SerializeField] private int m_Armor = 0;
        public int Armor => m_Armor;

        [Space]
        [SerializeField] private float m_ColliderRadius = 0.19f;
        public float ColliderRadius => m_ColliderRadius;

        [SerializeField] private Vector2 m_ColliderPosition = Vector2.zero;
        public Vector2 ColliderPosition => m_ColliderPosition;

        [Space]
        [SerializeField][Min(0.0f)] private float m_RandomSelectMovePointTime = 2f;
        public float RandomSelectMovePointTime => m_RandomSelectMovePointTime;

        [SerializeField][Min(0.0f)] private float m_FindNewTargetTime = 2f;
        public float FindNewTargetTime => m_FindNewTargetTime;

        [SerializeField][Min(0.0f)] private float m_AgressionRadius = 3f;
        public float AgressionRadius => m_AgressionRadius;

        [Header("Параметры врага")]
        [SerializeField] private int m_PlayerDamage = 1;
        public int PlayerDamage => m_PlayerDamage;

        [SerializeField] private int m_Gold = 10;
        public int Gold => m_Gold;

        [SerializeField] private int m_DeathScore = 100;
        public int DeathScore => m_DeathScore;
    }
}
