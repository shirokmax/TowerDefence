using UnityEngine;

namespace TowerDefence
{
    public class LevelDisplayController : MonoBehaviour
    {
        [SerializeField] private MapLevel[] m_Levels;

        private void Start()
        {
            var drawLevel = 0;
            int score = -1;

            while (score != 0 && 
                   drawLevel < m_Levels.Length && 
                   MapCompletion.Instance.TryCompletionDataIndex(drawLevel, out var episode, out score))
            {
                m_Levels[drawLevel].SetLevelData(episode, score);
                drawLevel++;
            }

            for (int i = drawLevel; i < m_Levels.Length; i++)
            {
                m_Levels[i].gameObject.SetActive(false);
            }
        }
    }
}
