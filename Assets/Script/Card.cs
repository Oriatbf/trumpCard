using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VInspector;

public class Card : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    [SerializeField] TextMeshProUGUI relicName,relicInfor;
    [SerializeField] Color[] rarityColor;
    [SerializeField] Image rarityImage,relicIcon;

    [SerializeField]private RectTransform rect;

    // Shop
    [SerializeField] bool ShopCard;

    [ShowIf("ShopCard")]
    [SerializeField] TextMeshProUGUI goldText;

    private RelicDatas relicData;

    public bool purchased;
    private int gold;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void Init(RelicDatas relicDatas)
    {
        relicData = relicDatas;
        relicName.text = relicDatas.name;
        relicInfor.text = relicDatas.description;
        int rarityIndex = 0;

        this.relicIcon.sprite = null;
        rarityImage.color= rarityColor[rarityIndex];
    }

    public void SetCard(RelicDatas relicDatas)
    {
       

        /*
        // Shop
        if (ShopCard)
        {
            switch (relicSO.rarity)
            {
                case RelicSO.Rarity.Common: gold = Random.Range(170, 200); break;
                case RelicSO.Rarity.Rare: gold = Random.Range(210, 250); break;
                case RelicSO.Rarity.Epic: gold = Random.Range(290, 350); break;
            }

            goldText.text = "<sprite=0> " + gold;
        }*/
    }
    

    // Shop
    public void SelectRelic()
    {
        CharacterRelicData.Inst.AddPlayerRelic(relicData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectRelic();
        RelicSelectManager.Inst.Close();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        float size = 1.15f;
        Debug.Log("닿음");
        rect.DOScale(new Vector3(size, size), 0.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        float size = 1f;
        Debug.Log("끝");
        rect.DOScale(new Vector3(size, size), 0.1f);
    }
    
}
