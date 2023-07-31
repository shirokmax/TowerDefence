using SpaceShooter;
using UnityEngine;

namespace TowerDefence
{
    public class UnitPath : MonoBehaviour
    {
        [SerializeField] CircleArea m_StartArea;
        public CircleArea StartArea => m_StartArea;

        [SerializeField] CircleArea[] m_Points;
        public int Length => m_Points.Length;
        public CircleArea this[int i] => m_Points[i];

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;

            for (int i = 0; i < m_Points.Length; i++)
            {
                if (m_Points[i] != null)
                {
                    UnityEditor.Handles.color = new Color(1, 1, 0, 0.3f);
                    UnityEditor.Handles.DrawSolidDisc(m_Points[i].transform.position, Vector3.forward, m_Points[i].Radius);

                    if (i + 1 < m_Points.Length && m_Points[i + 1] != null)
                        Gizmos.DrawLine(m_Points[i].transform.position, m_Points[i + 1].transform.position);
                }
            }
        }

#endif
    }
}
