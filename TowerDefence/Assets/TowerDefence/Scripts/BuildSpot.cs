using UnityEngine;

namespace TowerDefence
{
    public class BuildSpot : ClickSpot
    {
        [SerializeField] private TowerSettings[] m_BuildableTowers;
        public TowerSettings[] BuildableTowers => m_BuildableTowers;

        [SerializeField] private Transform m_UnitsStartHoldPoint;
        public Transform UnitsStartHoldPoint => m_UnitsStartHoldPoint;

        public void SetBuildableTowers(TowerSettings[] buildableTowers)
        {
            m_BuildableTowers = buildableTowers;
        }

        public void SetUnitsStartHoldPointPos(Transform point)
        {
            m_UnitsStartHoldPoint.transform.position = point.transform.position;
        }
    }
}
