using UnityEngine;

namespace TowerDefence
{
    public class CursorManager : MonoSingleton<CursorManager>
    {
        [SerializeField] private Texture2D m_DefaultTexture;
        [SerializeField] private Texture2D m_MouseDownTexture;

        [Space]
        [SerializeField] private Vector2 m_DefaultHotSpot = new Vector2 (4, 3);

        private void Start()
        {
            Cursor.SetCursor(m_DefaultTexture, m_DefaultHotSpot, CursorMode.Auto);
        }

        private void Update()
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                Cursor.SetCursor(m_MouseDownTexture, m_DefaultHotSpot, CursorMode.Auto);
            else
                Cursor.SetCursor(m_DefaultTexture, m_DefaultHotSpot, CursorMode.Auto);
        }
    }
}
