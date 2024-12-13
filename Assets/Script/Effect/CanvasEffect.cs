using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasEffect : MonoBehaviour
{
    [SerializeField] private Image image;

    [SerializeField] private TextMeshProUGUI text;
    void Start()
    {
        TimeManager.Inst.ChangeTimeSpeed(0f);
        image.DOFillAmount(1, .3f).OnComplete(() =>
        {
            text.DOFade(1, .5f).SetUpdate(true);
        }).SetUpdate(true);

        DOVirtual.DelayedCall(1.5f, () =>
        {
            text.DOFade(0, 0.3f).SetUpdate(true).OnComplete(() =>
            {
                image.DOFillAmount(0, .3f).SetUpdate(true).OnComplete(() =>
                {
                    TimeManager.Inst.ChangeTimeSpeed(1f);
                    Destroy(gameObject);
                });

            });
        }).SetUpdate(true);
    }
    
}
