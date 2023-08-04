using System;
using UnityEngine;
using SpaceShooter;

namespace TowerDefence
{
    public class MapCompletion : MonoSingleton<MapCompletion>
    {
        [Serializable]
        private class EpisodeStars
        {
            public Episode episode;
            public int stars;
        }

        public const string FILENAME = "Completion.dat";

        [SerializeField] private EpisodeStars[] m_CompletionData;

        private int m_TotalStars;
        public int TotalStars => m_TotalStars;

        protected override void Awake()
        {
            base.Awake();

            DataSaver<EpisodeStars[]>.TryLoad(FILENAME, ref m_CompletionData);
            m_TotalStars = CalculateTotalStars();
        }

        private int CalculateTotalStars()
        {
            int total = 0;

            foreach (var episodeStars in m_CompletionData)
                total += episodeStars.stars;

            return total;
        }

        public void SaveResult(Episode currentEpisode, int stars)
        {
            foreach (var item in m_CompletionData)
            {
                if (item.episode == currentEpisode)
                {
                    if (stars > item.stars)
                    {
                        item.stars = stars;

                        DataSaver<EpisodeStars[]>.Save(FILENAME, m_CompletionData);
                    }
                }
            }

            m_TotalStars = CalculateTotalStars();
        }

        public int GetEpisodeStars(Episode ep)
        {
            foreach (var data in m_CompletionData)
            {
                if (data.episode == ep)
                    return data.stars;
            }

            return 0;
        }

        public void ResetProgress()
        {
            foreach (var item in m_CompletionData)
                item.stars = 0;

            m_TotalStars = 0;
        }
    }
}
