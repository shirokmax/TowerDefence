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
        Waves,
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
        [SerializeField] private float m_AdditionalReferenceTime;

        private int m_LevelStars = 3;
        public int LevelStars => m_LevelStars;

        private ILevelCondition[] m_Conditions;

        private bool m_IsLevelCompleted;
        public bool IsLevelCompleted => m_IsLevelCompleted;

        private UnityEvent m_EventLevelComplete = new UnityEvent();
        public UnityEvent EventLevelComplete => m_EventLevelComplete;

        private float m_ReferenceTime;

        private float m_LevelTime;
        public float LevelTime => m_LevelTime;

        protected override void Awake()
        {
            base.Awake();

            m_Conditions = GetComponentsInChildren<ILevelCondition>();

            m_ReferenceTime += m_AdditionalReferenceTime;
        }

        private void Start()
        {
            Player.Instance.EventOnPlayerDeath.AddListener(OnLevelLose);

            Player.Instance.EventOnTakeDamage.AddListener(OnDamageTaken);

            void OnDamageTaken()
            {
                m_LevelStars--;
                Player.Instance.EventOnTakeDamage.RemoveListener(OnDamageTaken);
            }
        }

        private void Update()
        {
            if (m_IsLevelCompleted == false)
            {
                m_LevelTime += Time.deltaTime;

                CheckLevelConditions();
            }
        }

        private void OnLevelLose()
        {
            OnLevelComplete();

            m_LevelStars = 0;

            LevelSequenceController.Instance.FinishCurrentLevel(false);
        }

        private void OnLevelVictory()
        {
            OnLevelComplete();

            if (m_ReferenceTime <= m_LevelTime)
                m_LevelStars--;

            LevelSequenceController.Instance.FinishCurrentLevel(true);
        }

        private void OnLevelComplete()
        {
            m_IsLevelCompleted = true;
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
                    OnLevelLose();
            }

            if (numCompleted == m_Conditions.Length)
                OnLevelVictory();
        }

        public void SetReferenceTime(float time)
        {
            if (time < 0) return;

            m_ReferenceTime = time;
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
            {
                visualModel.UnitAnimator.SetBool("Attack", false);
            }
        }

        private static void DisableAll<T>() where T : MonoBehaviour
        {
            foreach (var obj in FindObjectsOfType<T>())
                obj.enabled = false;
        }
    }
}
