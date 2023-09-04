using UnityEngine.SceneManagement;
using UnityEngine;
using TowerDefence;

namespace SpaceShooter
{
    public class LevelSequenceController : MonoSingleton<LevelSequenceController>
    {
        [SerializeField] private Hero m_DefaultHero;

        public static Hero PlayerHero;

        public static string LevelMapSceneName = "LevelMap";

        public Episode CurrentEpisode { get; private set; }

        public int CurrentLevel { get; private set; }

        public bool LastLevelResult { get; private set; }

        public PlayerStatistics LevelStatistic { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            PlayerHero = m_DefaultHero;
        }

        public void StartEpisode(Episode ep)
        {
            CurrentEpisode = ep;
            CurrentLevel = 0;

            if (CurrentEpisode.Levels.Length == 0) return;

            LevelStatistic = new PlayerStatistics();

            SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void LoadLevelMap()
        {
            SceneManager.LoadScene(LevelMapSceneName);
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
        }

        public void FinishCurrentLevel(bool success)
        {
            LastLevelResult = success;

            CalculateLevelStatistics();

            UIResultPanel.Instance.ShowResults(LevelStatistic, success);

            if (success)
                MapCompletion.Instance.SaveResult(CurrentEpisode, LevelController.Instance.LevelStars);
        }

        public void AdvanceLevel()
        {
            // Переход на след. уровень
            CurrentLevel++;

            if (CurrentEpisode.Levels.Length <= CurrentLevel)
                SceneManager.LoadScene(LevelMapSceneName);
            else
                SceneManager.LoadScene(CurrentEpisode.Levels[CurrentLevel]);
        }

        private void CalculateLevelStatistics()
        {
            LevelStatistic.Reset();

            LevelStatistic.Stars = LevelController.Instance.LevelStars;
            LevelStatistic.Score = Player.Instance.Score;
            LevelStatistic.Kills = Player.Instance.NumKills;
            LevelStatistic.LevelTime = (int)LevelController.Instance.LevelTime;
        }
    }
}
