using System;
using UnityEngine;

namespace TowerDefence
{
    public enum UpgradeType
    {
        Default,   
        Percents,
        Unlock
    }

    [CreateAssetMenu]
    public sealed class UpgradeAsset : ScriptableObject
    {
        [Serializable]
        public class CostAndValue
        {
            //Стоймость апгрейда
            [SerializeField] private int cost;
            public int Cost => cost;

            //Значение, которое плюсуется к параметру после апгрейда
            [SerializeField] private float value;
            public float Value => value;
        }

        [SerializeField] private UpgradeType m_UpgradeType;
        public UpgradeType UpgradeType => m_UpgradeType;

        [SerializeField] private string m_UpgradeName;
        public string UpgradeName => m_UpgradeName;

        [SerializeField] private Sprite m_IconSprite;
        public Sprite IconSprite => m_IconSprite;

        [SerializeField] private CostAndValue[] m_CostsAndValues;
        public CostAndValue[] CostsAndValues => m_CostsAndValues;

        [SerializeField] private string m_Description;
        public string Description => m_Description;

        [SerializeField] private string m_PostValueDescription;
        public string PostValueDescription => m_PostValueDescription;
    }
}
