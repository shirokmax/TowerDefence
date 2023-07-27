using TowerDefence;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceShooter
{
    public enum LevelCondition
    {
        Score,
        Kills,
        Time,
        SurviveTime,
        CutScene
    }

    public interface ILevelCondition
    {
        LevelCondition Condition { get; }
        string Description { get; }
        bool IsCompleted { get; }
    }

    public class LevelController : MonoSingleton<LevelController>
    {
        // Звезды за прохождение уровня (от 1 до 3)
        public int LevelStars => 3;

        [SerializeField] private float m_ReferenceTime;
        public float ReferenceTime => m_ReferenceTime;

        private ILevelCondition[] m_Conditions;
        public ILevelCondition[] Conditions => m_Conditions;

        private bool m_IsLevelCompleted;
        public bool IsLevelCompleted => m_IsLevelCompleted;

        private UnityEvent m_EventLevelComplete = new UnityEvent();
        public UnityEvent EventLevelComplete => m_EventLevelComplete;

        private float m_LevelTime;
        public float LevelTime => m_LevelTime;

        protected override void Awake()
        {
            base.Awake();

            m_Conditions = GetComponentsInChildren<ILevelCondition>();
        }

        private void Start()
        {
            Player.Instance.EventOnPlayerDeath.AddListener(OnLoseLevel);
        }

        private void Update()
        {
            if (m_IsLevelCompleted == false)
            {
                m_LevelTime += Time.deltaTime;

                CheckLevelConditions();
            }
        }

        private void OnLoseLevel()
        {
            LevelSequenceController.Instance.FinishCurrentLevel(false);

            StopLevelActivity();

            m_EventLevelComplete.Invoke();
        }

        private void CheckLevelConditions()
        {
            if (m_Conditions.Length == 0)
                return;

            int numCompleted = 0;

            foreach(var condition in m_Conditions)
            {
                if (condition.IsCompleted)
                    numCompleted++;

                if (condition.Condition == LevelCondition.Time && condition.IsCompleted == false)
                {
                    m_IsLevelCompleted = true;

                    OnLoseLevel();
                }
            }

            if (numCompleted == m_Conditions.Length)
            {
                m_IsLevelCompleted = true;

                LevelSequenceController.Instance.FinishCurrentLevel(true);

                StopLevelActivity();

                m_EventLevelComplete.Invoke();
            }
        }

        private void StopLevelActivity()
        {
            DisableAll<Spawner>();
            DisableAll<Tower>();
            DisableAll<Projectile>();
            DisableAll<Unit>();
            DisableAll<AIController>();

            Player.Instance.enabled = false;
            UIHeroPanel.Instance.enabled = false;

            foreach (var visualModel in FindObjectsOfType<UnitVisualModel>())
                visualModel.UnitAnimator.SetBool("Attack", false);
        }

        private static void DisableAll<T>() where T : MonoBehaviour
        {
            foreach (var obj in FindObjectsOfType<T>())
                obj.enabled = false;
        }
    }
}
