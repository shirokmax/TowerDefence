using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TowerDefence
{
    public class ClickSpot : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Transform m_UnitsStartHoldPoint;
        public Transform UnitsStartHoldPoint => m_UnitsStartHoldPoint;

        protected static UnityEvent<Transform> m_EventOnSpotClick;
        public static UnityEvent<Transform> EventOnSpotClick => m_EventOnSpotClick;

        protected virtual void Awake()
        {
            m_EventOnSpotClick ??= new UnityEvent<Transform>();
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                m_EventOnSpotClick?.Invoke(transform.root);
        }
    }
}
