using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���������� ����� ��� ������� ������. �������� �� ������ � ������� � ��� ������������� �� ������� �������.
/// </summary>
[RequireComponent(typeof(Button))] 
public class UIButtonSFX : MonoBehaviour
{
    [SerializeField] private ImpactEffect m_EffectPrefab;

    private Button m_Button;

    private void Awake()
    {
        m_Button = GetComponent<Button>();

        m_Button.onClick.AddListener(Spawn);
    }

    public void Spawn()
    {
        if (m_EffectPrefab != null)
            DontDestroyOnLoad(Instantiate(m_EffectPrefab));
    }
}
