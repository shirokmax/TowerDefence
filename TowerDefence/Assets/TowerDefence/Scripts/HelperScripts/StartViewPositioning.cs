using UnityEngine;

namespace TowerDefence
{
    public class StartViewPositioning : MonoBehaviour
    {
        void Start()
        {
            //���������� ����������� �������� ������ ����-�����
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
        }
    }
}
