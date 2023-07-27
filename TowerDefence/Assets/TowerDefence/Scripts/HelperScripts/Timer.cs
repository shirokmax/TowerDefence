public class Timer
{
    private float m_CurrentTime;
    public float CurrentTime => m_CurrentTime;

    private float m_StartTime;
    public float StartTime => m_StartTime;

    public bool IsFinished => m_CurrentTime <= 0;

    public Timer(float startTime)
    {
        Start(startTime);
    }

    /// <summary>
    /// ������ StartTime � ��������� ������.
    /// </summary>
    /// <param name="startTime"��������� ����� �������.></param>
    public void Start(float startTime)
    {
        m_StartTime = startTime;

        Restart();
    }

    /// <summary>
    /// ������������� ������ �� ���������� �������� StartTime.
    /// </summary>
    public void Restart()
    {
        m_CurrentTime = m_StartTime;
    }

    /// <summary>
    /// �������� ������.
    /// </summary>
    public void Reset()
    {
        RemoveTime(m_StartTime);
    }

    /// <summary>
    /// �������� �������� ����� �� ��������.
    /// </summary>
    /// <param name="deltaTime">���������� �����.</param>
    public void RemoveTime(float deltaTime)
    {
        if (m_CurrentTime <= 0) return;

        m_CurrentTime -= deltaTime;
    }
}
