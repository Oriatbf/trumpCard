using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialCard : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    private Vector2 originalSizeDelta;
    private RectTransform rectTransform;
    [TextArea] [SerializeField] private string text;
    private MapTutorial mapTutorial;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        originalSizeDelta = rectTransform.sizeDelta;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.DOComplete();
        rectTransform.DOSizeDelta(originalSizeDelta * 1.2f, 0.25f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.DOComplete();
        rectTransform.DOSizeDelta(originalSizeDelta , 0.25f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       mapTutorial.Texting(text);
    }

    public void SetMapTutorial(MapTutorial _mapTutorial) => mapTutorial = _mapTutorial;
}
