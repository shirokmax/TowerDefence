using System;
using UnityEngine;

namespace TowerDefence
{
    [CreateAssetMenu]
    public sealed class UpgradeAsset : ScriptableObject
    {
        [Serializable]
        public class CostAndValue
        {
            //��������� ��������
            [SerializeField] private int cost;
            public int Cost => cost;

            //��������, ������� ��������� � ��������� ����� ��������
            [SerializeField] private float value;
            public float Value => value;
        }

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