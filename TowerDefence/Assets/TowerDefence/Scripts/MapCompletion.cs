using System;
using UnityEngine;
using SpaceShooter;

namespace TowerDefence
{
    public class MapCompletion : MonoSingleton<MapCompletion>
    {
        [Serializable]
        private class EpisodeScore
        {
            public Episode episode;
            public int stars;
        }

        public const string FILENAME = "Completion.dat";

        [SerializeField] private EpisodeScore[] m_CompletionData;

        protected override void Awake()
        {
            base.Awake();

            DataSaver<EpisodeScore[]>.TryLoad(FILENAME, ref m_CompletionData);
        }

        public bool TryCompletionDataIndex(int index, out Episode episode, out int stars)
        {
            if (index >= 0 && index < m_CompletionData.Length)
            {
                episode = m_CompletionData[index].episode;
                stars = m_CompletionData[index].stars;

                return true;
            }

            episode = null;
            stars = 0;

            return false;
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

                        DataSaver<EpisodeScore[]>.Save(FILENAME, m_CompletionData);
                    }
                }
            }
        }

        public void ResetProgress()
        {
            foreach (var item in m_CompletionData)
                item.stars = 0;
        }
    }
}
