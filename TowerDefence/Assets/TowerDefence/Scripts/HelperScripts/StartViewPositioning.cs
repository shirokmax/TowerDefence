using UnityEngine;

namespace TowerDefence
{
    public class StartViewPositioning : MonoBehaviour
    {
        void Start()
        {
            //Правильное отображение спрайтов поверх друг-друга
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
        }
    }
}
