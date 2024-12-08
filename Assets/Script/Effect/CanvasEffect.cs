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
        image.DOFillAmount(1, .3f).OnComplete(() =>
        {
            text.DOFade(1, .5f);
        });

        DOVirtual.DelayedCall(1.5f, () =>
        {
            text.DOFade(0, 0.3f).OnComplete(() =>
            {
                image.DOFillAmount(0, .3f);
            });

        });
    }
    
}
