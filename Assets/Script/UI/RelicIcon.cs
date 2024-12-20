using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RelicIcon : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerMoveHandler
{
    [SerializeField] private Image image;
    private RelicDatas relicData;
    private Card relicCard;
    private RectTransform _cardRect;

    private void Awake()
    {
      
    }

    public void Init(RelicDatas _relicData)
    {
        relicData = _relicData;
        image.sprite = _relicData.sprite;
        relicCard =RelicSelectManager.Inst.InstanceCard(relicData,transform.parent.parent,new Vector2(0.25f,0.25f));
        _cardRect = relicCard.GetComponent<RectTransform>();
        relicCard.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        relicCard.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        relicCard.gameObject.SetActive(false);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (_cardRect != null)
        {
            _cardRect.transform.position = Input.mousePosition;
            if (_cardRect.transform.position.y <= 300)
            {
                _cardRect.pivot = new Vector2(0, 0);
            }
            else
            {
                _cardRect.pivot = new Vector2(.5f, 1f);
            }
        }
    }
}
