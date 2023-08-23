using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TowerDefence
{
    [RequireComponent(typeof(Image))]
    public class UIClickProtection : MonoSingleton<UIClickProtection>, IPointerDownHandler
    {
        private Image m_ProtectionImage;

        private Action<Vector2, bool> m_OnClickAction;

        protected override void Awake()
        {
            base.Awake();

            m_ProtectionImage = GetComponent<Image>();
            m_ProtectionImage.enabled = false;
        }

        private void Update()
        {
            if (m_OnClickAction != null)
            {
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
                {
                    m_ProtectionImage.enabled = false;
                    m_OnClickAction(Vector2.zero, false);
                    m_OnClickAction = null;
                }
            }
        }

        public void Activate(Action<Vector2, bool> mouseAction)
        {
            m_ProtectionImage.enabled = true;
            m_OnClickAction = mouseAction;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                m_ProtectionImage.enabled = false;
                m_OnClickAction(eventData.pressPosition, true);
                m_OnClickAction = null;
            }
        }
    }
}
