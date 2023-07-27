using UnityEngine;

public class ImpactEffect : MonoBehaviour
{
    [SerializeField][Min(0)] private float m_LifeTime;
    public float LifeTime => m_LifeTime;

    private float m_LifeTimer;
    public float LifeTimer => m_LifeTimer;

    protected virtual void Update()
    {
        if (m_LifeTimer < m_LifeTime)
            m_LifeTimer += Time.deltaTime;
        else 
            Destroy(gameObject);
    }

    public void SetLifeTime(float time)
    {
        if (time < 0) return;

        m_LifeTime = time;
    }
}
