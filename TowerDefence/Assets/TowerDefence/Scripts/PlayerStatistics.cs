namespace SpaceShooter
{
    public class PlayerStatistics
    {
        public int Stars;
        public int Score;
        public int Kills;
        public int LevelTime;

        public PlayerStatistics()
        {
            Reset();
        }

        public void Reset()
        {
            Stars = 0;
            Score = 0;
            Kills = 0;
            LevelTime = 0;
        }
    }
}
