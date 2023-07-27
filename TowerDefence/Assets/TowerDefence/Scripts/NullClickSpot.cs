using UnityEngine.EventSystems;

namespace TowerDefence
{
    public class NullClickSpot : ClickSpot
    {
        public override void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                m_EventOnSpotClick.Invoke(null);
        }
    }
}
