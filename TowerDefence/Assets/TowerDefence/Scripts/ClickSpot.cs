using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TowerDefence
{
    public class ClickSpot : MonoBehaviour, IPointerDownHandler
    {
        protected static UnityEvent<ClickSpot> m_EventOnSpotClick;
        public static UnityEvent<ClickSpot> EventOnSpotClick => m_EventOnSpotClick;

        protected virtual void Awake()
        {
            m_EventOnSpotClick ??= new UnityEvent<ClickSpot>();
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                m_EventOnSpotClick?.Invoke(this);
        }
    }
}
