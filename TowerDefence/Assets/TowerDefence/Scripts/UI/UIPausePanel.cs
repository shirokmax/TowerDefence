using UnityEngine;
using SpaceShooter;

namespace TowerDefence
{
    public class UIPausePanel : MonoBehaviour
    {
        public void OnQuitLevelButtonClick()
        {
            LevelSequenceController.Instance.LoadLevelMap();
        }

        private void OnEnable()
        {
            Time.timeScale = 0f;
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
        }
    }
}
