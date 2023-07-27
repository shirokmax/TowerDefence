using UnityEngine;
using UnityEngine.Events;
using TowerDefence;

namespace SpaceShooter
{
    public enum AIBehaviour
    {
        None,
        HoldPosition,
        AreaPatrol,
        PathPatrol,
        PathMove,
        PlayerControl
    }

    [RequireComponent(typeof(Unit))]
    public class AIController : MonoBehaviour
    {
        [SerializeField] private AIBehaviour m_AIBehaviour = AIBehaviour.HoldPosition;

        [Space]
        [SerializeField] private Transform m_HoldPoint;
        [SerializeField] private CircleArea m_PatrolArea;
        [SerializeField] private UnitPath m_Path;

        [Space]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_SpeedControl = 1f;

        [Space]
        [SerializeField][Min(0.0f)] private float m_RandomSelectMovePointTime = 2f;
        [SerializeField][Min(0.0f)] private float m_FindNewTargetTime = 2f;

        [Space]
        [SerializeField][Min(0.0f)] private float m_AgressionRadius = 2f;

        [Space]
        [SerializeField] private UnityEvent m_EventOnPathEnd;

        [Space]
        [SerializeField] private ImpactEffect m_MovementPointVFXPrefab;
        [SerializeField] private ImpactEffect m_MovementPointSFXPrefab;

        [Space]
        [SerializeField] private ImpactEffect m_InvalidMovementPointVFXPrefab;
        [SerializeField] private ImpactEffect m_InvalidMovementPointSFXPrefab;

        public const string ROAD_TAG = "Road";

        private const float MOVE_POSITION_THRESHOLD = 0.03f;

        private Unit m_Unit;

        private Vector2 m_MovePosition;

        private Unit m_SelectedTarget;
        public Unit SelectedTarget => m_SelectedTarget;

        private Timer m_RandomizeDirectionTimer;
        private Timer m_FindNewTargetTimer;
        private Timer m_ConfirmationVoiceTimer;

        private int m_PathIndex;

        private bool m_MovingToPlayerTarget;

        private void Awake()
        {
            m_Unit = GetComponent<Unit>();

            m_MovePosition = transform.position;

            SetStartBehaivor();

            InitTimers();
        }

        private void SetStartBehaivor()
        {
            switch (m_AIBehaviour)
            {
                case AIBehaviour.AreaPatrol:
                    SetAreaPatrolBehaviour(m_PatrolArea);
                    break;

                case AIBehaviour.PathPatrol:
                    SetPathBehaviour(m_Path, AIBehaviour.PathPatrol);
                    break;

                case AIBehaviour.PathMove:
                    SetPathBehaviour(m_Path, AIBehaviour.PathMove);
                    break;
            }
        }

        private void Update()
        {
            if (m_AIBehaviour == AIBehaviour.None) return;

            if (m_AIBehaviour == AIBehaviour.AreaPatrol && m_PatrolArea == null)
            {
                Debug.LogWarning("AIController: PatrolArea = null! AIBehaviour has been set to None." + "(" + transform.root.name + ")");
                SetNoneBehaviour();
                return;
            }
            if ((m_AIBehaviour == AIBehaviour.PathMove || m_AIBehaviour == AIBehaviour.PathPatrol) && (m_Path == null || m_Path.Length == 0))
            {
                Debug.LogWarning("AIController: Path = null or Path Length = 0! AIBehaviour has been set to None." + "(" + transform.root.name + ")");
                SetNoneBehaviour();
                return;
            }

            UpdateAI();
            UpdateTimers();
        }

        private void UpdateAI()
        {
            ActionFindNewMovePosition();
            ActionControlUnit();
            ActionFindNewAttackTarget();
            ActionAttack();
        }

        #region Actions
        private void ActionFindNewMovePosition()
        {
            if (m_AIBehaviour == AIBehaviour.PlayerControl)
            {
                if (Input.GetMouseButtonUp(1))
                {
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                    if (hit.collider != null && hit.collider.tag == ROAD_TAG)
                    {
                        m_MovePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                        if (m_SelectedTarget != null)
                            m_SelectedTarget.AIController.SetTarget(null);

                        m_SelectedTarget = null;
                        m_MovingToPlayerTarget = true;

                        if (m_MovementPointVFXPrefab != null)
                            Instantiate(m_MovementPointVFXPrefab, m_MovePosition, Quaternion.identity);

                        if (m_MovementPointSFXPrefab != null)
                            Instantiate(m_MovementPointSFXPrefab);

                        if (m_Unit.VisualModel.ConfirmationVoiceSFXPrefabs.Length > 0)
                        {
                            if (m_ConfirmationVoiceTimer.IsFinished)
                            {
                                int index = Random.Range(0, m_Unit.VisualModel.ConfirmationVoiceSFXPrefabs.Length);
                                Instantiate(m_Unit.VisualModel.ConfirmationVoiceSFXPrefabs[index]);

                                m_ConfirmationVoiceTimer.Restart();
                            }
                        }
                    }
                    else
                    {
                        if (m_InvalidMovementPointVFXPrefab != null)
                            Instantiate(m_InvalidMovementPointVFXPrefab, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);

                        if (m_InvalidMovementPointSFXPrefab != null)
                            Instantiate(m_InvalidMovementPointSFXPrefab);
                    }
                }

                if (InMovePosition())
                    m_MovingToPlayerTarget = false;
            }

            if (m_SelectedTarget == null)
            {
                if (m_AIBehaviour == AIBehaviour.HoldPosition)
                {
                    if (InHoldPosition() == false)
                    {
                        m_MovePosition = m_HoldPoint.position;
                    }
                }

                if (m_AIBehaviour == AIBehaviour.AreaPatrol)
                {
                    if (InsidePatrolArea() == true)
                    {
                        if (m_RandomizeDirectionTimer.IsFinished || InMovePosition())
                        {
                            m_MovePosition = m_PatrolArea.GetRandomInsideZone();

                            m_RandomizeDirectionTimer.Restart();
                        }
                    }
                    else
                    {
                        m_MovePosition = m_PatrolArea.transform.position;
                    }
                }

                if (m_AIBehaviour == AIBehaviour.PathMove)
                {
                    if (InMovePosition())
                        GetNewPathPoint();
                }

                if (m_AIBehaviour == AIBehaviour.PathPatrol)
                {
                    if (InMovePosition())
                        GetNewPatrolPathPoint();
                }
            }
        }

        private void ActionControlUnit()
        {
            if ((m_Unit.Team == Team.Enemy && m_SelectedTarget != null) ||
                (m_AIBehaviour == AIBehaviour.PlayerControl && InMovePosition()) ||
                 m_AIBehaviour == AIBehaviour.HoldPosition && InMovePosition())
            {
                m_Unit.SpeedControl = 0;
            }
            else
            {
                m_Unit.SpeedControl = m_SpeedControl;   
            }

            Vector2 dir;

            //Для того, чтобы юнит "смотрел" на своего врага при сражении
            if (m_SelectedTarget != null && m_Unit.SpeedControl == 0)
                dir = (Vector2)m_SelectedTarget.transform.position - (Vector2)transform.position;
            else
                dir = m_MovePosition - (Vector2)transform.position;

            transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);
        }

        private void ActionFindNewAttackTarget()
        {
            if (m_Unit.Team == Team.Player)
            {
                if (m_SelectedTarget != null) return;
                if (m_AIBehaviour == AIBehaviour.PlayerControl && m_MovingToPlayerTarget == true) return;

                Unit newTarget = FindNearestUnitTarget();

                if (newTarget != null)
                {
                    m_SelectedTarget = newTarget;
                    newTarget.AIController.SetTarget(m_Unit);

                    SetMovePositionToSelectedTarget();
                } 
            }
        }

        private void ActionAttack()
        {
            if (m_SelectedTarget == null)
            {
                m_Unit.AnimationEvents.SetTargetUnit(null);
                m_Unit.VisualModel.UnitAnimator.SetBool("Attack", false);

                return;
            }
            else
            {
                if ((m_Unit.Team == Team.Player && InMovePosition()) ||
                    (m_Unit.Team == Team.Enemy && m_SelectedTarget.AIController.InMovePosition()))
                {
                    m_Unit.AnimationEvents.SetTargetUnit(m_SelectedTarget);
                    m_Unit.VisualModel.UnitAnimator.SetBool("Attack", true);
                }
                else
                {
                    m_Unit.AnimationEvents.SetTargetUnit(null);
                    m_Unit.VisualModel.UnitAnimator.SetBool("Attack", false);
                }
            }
        }

        #endregion

        #region Helper Methods
        private void SetMovePositionToSelectedTarget()
        {
            if (m_AIBehaviour == AIBehaviour.PlayerControl && m_MovingToPlayerTarget) return;

            float attackDist = m_Unit.MeleeAttackRangeRadius > SelectedTarget.MeleeAttackRangeRadius ? m_Unit.MeleeAttackRangeRadius : SelectedTarget.MeleeAttackRangeRadius;

            if (m_Unit.transform.position.x > m_SelectedTarget.transform.position.x)
                m_MovePosition = new Vector3(m_SelectedTarget.transform.position.x + attackDist, m_SelectedTarget.transform.position.y, m_SelectedTarget.transform.position.z);
            else
                m_MovePosition = new Vector3(m_SelectedTarget.transform.position.x - attackDist, m_SelectedTarget.transform.position.y, m_SelectedTarget.transform.position.z);
        }

        private void GetNewPathPoint()
        {
            m_PathIndex++;

            if (m_PathIndex < m_Path.Length)
            {
                m_MovePosition = m_Path[m_PathIndex].GetRandomInsideZone();
            }
            else
            {
                m_EventOnPathEnd?.Invoke();
                Destroy(gameObject);
            }
        }

        private void GetNewPatrolPathPoint()
        {
            m_PathIndex++;

            if (m_PathIndex >= m_Path.Length)
            {
                m_PathIndex = 0;
            }

            m_MovePosition = m_Path[m_PathIndex].GetRandomInsideZone();
        }

        private Unit FindNearestUnitTarget()
        {
            Unit potentialTarget = null;

            float minDist = float.MaxValue;

            foreach (Unit unit in Unit.AllUnits)
            {
                if (unit == m_Unit) continue;

                if (unit.MoveType != MovementType.Walking) continue;
                if (unit.Team == Team.Neutral) continue;
                if (unit.Team == m_Unit.Team) continue;
                if (unit.AIController.SelectedTarget != null) continue;

                float dist = Vector2.Distance(m_Unit.transform.position, unit.transform.position);

                if (dist < minDist && dist <= m_AgressionRadius)
                {
                    potentialTarget = unit;
                    minDist = dist;
                }
            }

            return potentialTarget;
        }

        #endregion

        #region Set Behaviours and Params
        public void ApplyUnitSettings(UnitSettings settings)
        {
            m_SpeedControl = settings.SpeedControl;

            m_RandomSelectMovePointTime = settings.RandomSelectMovePointTime;
            m_FindNewTargetTime = settings.FindNewTargetTime;

            m_AgressionRadius = settings.AgressionRadius;
        }

        public void SetTarget(Unit unit)
        {
            if (unit != null)
                m_SelectedTarget = unit;
            else
                m_SelectedTarget = null;
        }

        public bool SetHoldPositionBehaviour(Transform holdPoint)
        {
            if (holdPoint == null)
            {
                Debug.LogWarning("AIController: HoldPoint = null! AIBehaviour has been set to None." + "(" + transform.root.name + ")");
                SetNoneBehaviour();
                return false;
            }

            m_AIBehaviour = AIBehaviour.HoldPosition;
            m_HoldPoint = holdPoint;

            return true;
        }

        public bool SetAreaPatrolBehaviour(CircleArea area)
        {
            if (area == null)
            {
                Debug.LogWarning("AIController: PatrolArea = null! AIBehaviour has been set to None." + "(" + transform.root.name + ")");
                SetNoneBehaviour();
                return false;
            }

            m_AIBehaviour = AIBehaviour.AreaPatrol;
            m_PatrolArea = area;

            return true;
        }

        /// <summary>
        /// Устанавливает один из режимов следования по пути.
        /// </summary>
        /// <param name="path">Путь</param>
        /// <param name="pathBehaviour">Поведение, связанное с путем.</param>
        /// <returns></returns>
        public bool SetPathBehaviour(UnitPath path, AIBehaviour pathBehaviour)
        {
            if (path == null || path.Length == 0)
            {
                Debug.LogWarning("AIController: Path = null or Path Length = 0! AIBehaviour has been set to None." + "(" + transform.root.name + ")");
                SetNoneBehaviour();
                return false;
            }

            if (pathBehaviour == AIBehaviour.PathPatrol)
                m_AIBehaviour = AIBehaviour.PathPatrol;
            else
                m_AIBehaviour = AIBehaviour.PathMove;

            m_Path = path;
            m_PathIndex = 0;
            m_MovePosition = m_Path[m_PathIndex].GetRandomInsideZone();

            return true;
        }

        public void SetNoneBehaviour()
        {
            m_AIBehaviour = AIBehaviour.None;
        }

        #endregion

        #region Zone Checks
        public bool InMovePosition()
        {
            return (m_MovePosition - (Vector2)transform.position).sqrMagnitude <= MOVE_POSITION_THRESHOLD * MOVE_POSITION_THRESHOLD;
        }

        private bool InHoldPosition()
        {
            return ((Vector2)m_HoldPoint.position - (Vector2)transform.position).sqrMagnitude <= MOVE_POSITION_THRESHOLD * MOVE_POSITION_THRESHOLD;
        }

        private bool InsidePatrolArea()
        {
            float dist = ((Vector2)m_PatrolArea.transform.position - (Vector2)transform.position).sqrMagnitude;

            return dist <= m_PatrolArea.Radius * m_PatrolArea.Radius;
        }

        #endregion

        #region Timers
        private void InitTimers()
        {
            m_RandomizeDirectionTimer = new Timer(m_RandomSelectMovePointTime);
            m_RandomizeDirectionTimer.Reset();

            m_FindNewTargetTimer = new Timer(m_FindNewTargetTime);
            m_FindNewTargetTimer.Reset();

            m_ConfirmationVoiceTimer = new Timer(m_Unit.VisualModel.ConfirmationVoiceCooldown);
            m_ConfirmationVoiceTimer.Reset();
        }

        private void UpdateTimers()
        {
            m_RandomizeDirectionTimer.RemoveTime(Time.deltaTime);
            m_FindNewTargetTimer.RemoveTime(Time.deltaTime);
            m_ConfirmationVoiceTimer.RemoveTime(Time.deltaTime);
        }

        #endregion

        #region Unity Editor

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (m_SelectedTarget != null)
                UnityEditor.Handles.color = new Color(1, 0, 0, 0.1f);
            else
                UnityEditor.Handles.color = new Color(0, 1, 0, 0.1f);

            // Точка движения
            UnityEditor.Handles.DrawSolidDisc(m_MovePosition, Vector3.forward, MOVE_POSITION_THRESHOLD);

            // Радиус агра
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, m_AgressionRadius);

            if (m_AIBehaviour == AIBehaviour.AreaPatrol && m_PatrolArea != null)
            {
                // Радиус зоны патруля
                UnityEditor.Handles.color = new Color(1, 1, 0, 0.02f);
                UnityEditor.Handles.DrawSolidDisc(m_PatrolArea.transform.position, Vector3.forward, m_PatrolArea.Radius);
            }
        }
#endif

        #endregion
    }
}
