using System;
using UnityEngine;

namespace TowerDefence
{
    public class Upgrades : MonoSingleton<Upgrades>
    {
        [Serializable]
        private class UpgradeSave
        {
            public UpgradeAsset asset;
            public int level = 0;
        }

        public const string FILENAME = "upgrades.dat";

        [SerializeField] private UpgradeSave[] m_Saves;

        protected override void Awake()
        {
            base.Awake();

            DataSaver<UpgradeSave[]>.TryLoad(FILENAME, ref m_Saves);
        }

        public static void BuyUpgrade(UpgradeAsset asset)
        {
            foreach (var upgrade in Instance.m_Saves)
            {
                if (upgrade.asset == asset)
                {
                    upgrade.level++;
                    DataSaver<UpgradeSave[]>.Save(FILENAME, Instance.m_Saves);
                }
            }
        }

        public static int GetUpgradeLevel(UpgradeAsset asset)
        {
            foreach (var upgrade in Instance.m_Saves)
            {
                if (upgrade.asset == asset)
                    return upgrade.level;
            }
            
            return 0;
        }

        public static float GetCurrentUpgradeValue(UpgradeAsset asset)
        {
            float totalValue = 0;
            int level = GetUpgradeLevel(asset);

            foreach (var upgrade in Instance.m_Saves)
            {
                if (upgrade.asset == asset)
                {
                    for (int i = 0; i < level; i++)
                        totalValue += upgrade.asset.CostsAndValues[i].Value;

                    return totalValue;
                }
            }

            return 0f;
        }

        public static float GetUpgradeValueByLevel(UpgradeAsset asset, int level)
        {
            float totalValue = 0;

            foreach (var upgrade in Instance.m_Saves)
            {
                if (upgrade.asset == asset)
                {
                    for (int i = 0; i < level; i++)
                        totalValue += upgrade.asset.CostsAndValues[i].Value;

                    return totalValue;
                }
            }

            return 0f;
        }

        public static int TotalStarsSpent()
        {
            int result = 0;

            foreach (var save in Instance.m_Saves)
            {
                for (int i = 0; i < save.level; i++)
                    result += save.asset.CostsAndValues[i].Cost;
            }

            return result;
        }

        public static void Reset()
        {
            FileHandler.Reset(FILENAME);

            foreach(var save in Instance.m_Saves)
                save.level = 0;
        }
    }
}
