using UnityEngine;

namespace SpaceShooter
{
    [RequireComponent(typeof(CircleArea))]
    public abstract class Spawner : MonoBehaviour
    {
        public enum SpawnMode
        {
            Single,
            Loop,
            LoopUntil
        }

        [SerializeField] protected SpawnMode m_SpawnMode;

        [SerializeField][Min(0)] protected int m_SpawnCount;
        [SerializeField][Min(0.0f)] protected float m_RespawnTime;
        [SerializeField][Min(0.0f)] protected float m_FirstSpawnDelay;

        /// <summary>
        /// Макс. число одновременно существующих заспавненных объектов. Если лимит равен нулю, то объекты спавнятся без лимита.
        /// </summary>
        [SerializeField][Min(0)] protected int m_SpawnCountLimit;

        protected CircleArea m_SpawnArea;

        protected int m_CurrentSpawnedCount;

        protected float m_Timer;
        protected float m_FirstSpawnTimer;
        protected bool m_CanSpawn => m_Timer <= 0;

        protected bool m_SingleSpawned;

        protected virtual void Awake()
        {
            m_SpawnArea = GetComponent<CircleArea>();
            m_FirstSpawnTimer = m_FirstSpawnDelay;
        }

        protected virtual void Update()
        {
            if (m_FirstSpawnTimer > 0)
            {
                m_FirstSpawnTimer -= Time.deltaTime;
            }
            else
            {
                if (m_SpawnMode == SpawnMode.Single && m_SingleSpawned == false)
                {
                    Spawn();
                    m_SingleSpawned = true;
                }

                if (m_SpawnMode == SpawnMode.Loop ||
                    m_SpawnMode == SpawnMode.LoopUntil)
                {
                    UpdateTimer();

                    if (m_CanSpawn)
                        Spawn();
                }
            }
        }

        protected abstract void Spawn();

        protected void OnDestroyEntity()
        {
            if (m_SpawnMode == SpawnMode.LoopUntil)
                return;

            m_CurrentSpawnedCount--;
        }

        protected virtual void UpdateTimer()
        {
            if (m_Timer > 0)
                m_Timer -= Time.deltaTime;
        }
    }
}
