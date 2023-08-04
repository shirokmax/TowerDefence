using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{

    [RequireComponent(typeof(MapLevel))]
    public class BranchLevel : MonoBehaviour
    {
        [SerializeField] private MapLevel m_RootLevel;
        [SerializeField] private Text m_StarsText;
        [SerializeField] private int m_NeedStars;

        public void TryActivate()
        {
            gameObject.SetActive(m_RootLevel.IsComplete);

            if (m_NeedStars <= MapCompletion.Instance.TotalStars)
            {
                m_StarsText.transform.parent.gameObject.SetActive(false);
                GetComponent<MapLevel>().Initialize();
            }
            else
            {
                m_StarsText.text = m_NeedStars.ToString();
            }
        }
    }
}
