using UnityEngine;
using UnityEngine.Events;
using TowerDefence;
using System.Collections;

namespace SpaceShooter
{
    public class Player : MonoSingleton<Player>
    {
        #region Properties
        [SerializeField] private int m_NumLives;
        public int NumLives => m_NumLives;

        [SerializeField] private int m_Gold;
        public int Gold => m_Gold;

        [Space]
        [SerializeField] private Tower m_TowerPrefab;
        [Min(0f)][SerializeField] private float m_TowerBuildTime;
        [SerializeField] private ProgressBarImpactEffect m_TowerBuildProgressVFXPrefab;
        [SerializeField] private ImpactEffect[] m_TowerBuildCompleteSFXPrefabs;

        [Space]
        [SerializeField] private ImpactEffect m_TakeDamageSFXPrefab;

        [Space]
        [SerializeField] private Transform m_HeroRespawnPoint;

        private Hero m_ActiveHero;
        public Hero ActiveHero => m_ActiveHero;

        private Timer m_HeroRespawnTimer;
        public Timer HeroRespawnTimer => m_HeroRespawnTimer;

        private int m_Score;
        public int Score => m_Score;

        private int m_NumKills;
        public int NumKills => m_NumKills;

        private UnityEvent m_EventOnPlayerDeath = new UnityEvent();
        public UnityEvent EventOnPlayerDeath => m_EventOnPlayerDeath;

        private UnityEvent m_EventOnTakeDamage = new UnityEvent();
        public UnityEvent EventOnTakeDamage => m_EventOnTakeDamage;

        private UnityEvent m_EventOnChangeLives = new UnityEvent();
        private UnityEvent m_EventOnChangeGold = new UnityEvent();

        private UnityEvent m_EventOnRespawnHero = new UnityEvent();
        public UnityEvent EventOnRespawnHero => m_EventOnRespawnHero;

        #endregion

        #region Events Subscribe
        public void GoldChangeSubscribe(UnityAction action)
        {
            m_EventOnChangeGold.AddListener(action);
            m_EventOnChangeGold.Invoke();
        }

        public void LivesChangeSubscribe(UnityAction action)
        {
            m_EventOnChangeLives.AddListener(action);
            m_EventOnChangeLives.Invoke();
        }

        #endregion

        protected override void Awake()
        {
            base.Awake();

            if (m_ActiveHero != null)
                Destroy(m_ActiveHero.gameObject);

            RespawnHero();

            if (LevelSequenceController.PlayerHero != null)
                m_HeroRespawnTimer = new Timer(LevelSequenceController.PlayerHero.RespawnCooldown);
        }

        private void Update()
        {
            if (LevelSequenceController.PlayerHero == null) return;

            if (m_ActiveHero == null)
            {
                m_HeroRespawnTimer.RemoveTime(Time.deltaTime);

                if (m_HeroRespawnTimer.IsFinished == true)
                {
                    RespawnHero();

                    if (ActiveHero.VisualModel.RespawnVoiceSFXPrefabs.Length > 0)
                    {
                        int index = Random.Range(0, ActiveHero.VisualModel.RespawnVoiceSFXPrefabs.Length);
                        Instantiate(ActiveHero.VisualModel.RespawnVoiceSFXPrefabs[index]);
                    }
                }
            }
        }

        private void RespawnHero()
        {
            if (LevelSequenceController.PlayerHero == null) return;

            m_ActiveHero = Instantiate(LevelSequenceController.PlayerHero, m_HeroRespawnPoint.position, Quaternion.identity);
            m_ActiveHero.EventOnDeath.AddListener(() => m_HeroRespawnTimer.Restart());

            m_EventOnRespawnHero.Invoke();
        }

        public void RemoveLives(int damage)
        {
            m_NumLives -= damage;

            if (m_NumLives <= 0)
            {
                m_NumLives = 0;
                m_EventOnPlayerDeath?.Invoke();
            }

            m_EventOnChangeLives?.Invoke();
            m_EventOnTakeDamage?.Invoke();

            if (m_TakeDamageSFXPrefab != null)
                Instantiate(m_TakeDamageSFXPrefab);
        }

        public void AddGold(int value)
        {
            m_Gold += value;

            m_EventOnChangeGold?.Invoke();
        }

        public bool RemoveGold(int value)
        {
            if (m_Gold - value < 0) return false;

            m_Gold -= value;

            m_EventOnChangeGold?.Invoke();

            return true;
        }

        public void TryBuild(TowerSettings towerProps, Transform spot)
        {
            if (RemoveGold(towerProps.GoldCost) == true)
            {
                if (m_TowerBuildProgressVFXPrefab != null)
                {
                    var buildProgress = Instantiate(m_TowerBuildProgressVFXPrefab, spot.position, Quaternion.identity);
                    buildProgress.SetLifeTime(m_TowerBuildTime);

                    StartCoroutine(BuildTowerWithTime(towerProps, spot));
                }
                else
                {
                    BuildTower(towerProps, spot);
                    SpawnTowerBuildCompleteSFX();
                }

                spot.gameObject.SetActive(false);
                ClickSpot.EventOnSpotClick.Invoke(null);
            }
        }

        private void BuildTower(TowerSettings towerProps, Transform spot)
        {
            Tower tower = Instantiate(m_TowerPrefab, spot.position, Quaternion.identity);
            tower.ApplySettings(towerProps);

            //Выключаем скрипт башни, если она построилась после конца уровня
            if (LevelController.Instance.IsLevelCompleted == true)
            {
                tower.enabled = false;
                return;
            }

            if (towerProps.SpawnUnits == true)
                tower.UnitSpawner.SetUnitsHoldPointPosition(spot.GetComponentInChildren<ClickSpot>().UnitsStartHoldPoint);
        }

        private IEnumerator BuildTowerWithTime(TowerSettings towerProps, Transform spot)
        {
            yield return new WaitForSeconds(m_TowerBuildTime);       

            BuildTower(towerProps, spot);
            SpawnTowerBuildCompleteSFX();
        }

        private void SpawnTowerBuildCompleteSFX()
        {
            if (m_TowerBuildCompleteSFXPrefabs.Length > 0)
            {
                int index = Random.Range(0, m_TowerBuildCompleteSFXPrefabs.Length);
                Instantiate(m_TowerBuildCompleteSFXPrefabs[index]);
            }
        }

        #region Score
        public void AddScore(int num)
        {
            m_Score += num;
        }

        public void AddKill()
        {
            m_NumKills++;
        }

        #endregion
    }
}
