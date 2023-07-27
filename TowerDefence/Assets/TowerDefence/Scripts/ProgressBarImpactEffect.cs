using UnityEngine;
using UnityEngine.UI;

public class ProgressBarImpactEffect : ImpactEffect
{
    [SerializeField] private Image m_ProgressBarImage;

    protected override void Update()
    {
        base.Update();

        m_ProgressBarImage.fillAmount = LifeTimer / LifeTime;
    }
}
