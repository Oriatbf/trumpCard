using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VInspector;
using VInspector.Libs;
using Random = UnityEngine.Random;

public class Card : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    [SerializeField] TextMeshProUGUI relicName,relicInfor;
    [SerializeField] Color[] rarityColor;
    [SerializeField] Image rarityImage,relicIcon,cardImage;

    [SerializeField]private RectTransform rect;

    // Shop
    [SerializeField] bool ShopCard;

    [ShowIf("ShopCard")]
    [SerializeField] TextMeshProUGUI goldText;

    private bool isSold = false;

    private RelicDatas relicData;
    

    public bool purchased;
    private int gold;
    private Vector2 originalScale;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        originalScale = transform.localScale;
    }
    
    public void Init(RelicDatas relicDatas)
    {
        Init(relicDatas,originalScale);
    }

    public void Init(RelicDatas relicDatas, Vector2 scale,bool raycastTarget = true)
    {
        if(isSold) return;
        transform.localScale = scale;
        originalScale = scale;
        relicData = relicDatas;
        relicName.text = relicDatas.name;
        relicInfor.text = relicDatas.description; 
        int rarityIndex = relicDatas.rarity.ToInt();
        
        relicIcon.sprite = relicDatas.sprite;
        
        
        rarityImage.color= rarityColor[rarityIndex];
        cardImage.raycastTarget = raycastTarget;
    }
    
    

    public void SetShop()
    {
        if(isSold) return;
        cardImage.raycastTarget = false;
        if (goldText == null || relicData == null)
        {
            Debug.LogError("텍스트 또는 유물 데이터가 없음");
            return;
        }
        switch (relicData.rarity)
        {
            case Rarity.Common: gold = Random.Range(170, 200); break;
            case Rarity.Rare: gold = Random.Range(210, 250); break;
            case Rarity.Epic: gold = Random.Range(290, 350); break;
        }
        goldText.text = "<sprite=0> " + gold;
        
    }

    public void ResetShop()
    {
        isSold = false;
    }
    

    // Shop
    public void SelectRelic()
    {
        if(isSold) return;
        if (ShopCard)
        {
            if(TopUIController.Inst.CurrentGold()<gold)return;
            TopUIController.Inst.GetGold(-gold);
            goldText.text = "Sold";
            isSold = true;
        }
        TopUIController.Inst.InstanceRelicIcon(relicData,true);
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!ShopCard)
        {
            SelectRelic();
            RelicSelectController.Inst.Close();
        }
     
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!ShopCard)
        {
            rect.DOScale(originalScale * 1.05f, 0.15f).SetUpdate(true);
        }
       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!ShopCard)
        {
            rect.DOScale(originalScale, 0.15f).SetUpdate(true);
        }
    }
    
}
