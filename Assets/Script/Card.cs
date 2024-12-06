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

    private RectTransform rect;

    // Shop
    [SerializeField] bool ShopCard;

    [ShowIf("ShopCard")]
    [SerializeField] TextMeshProUGUI goldText;

    public bool purchased;
    private int gold;

    public void SetCard(RelicDatas relicDatas)
    {
        relicName.text = relicDatas.name;
        relicInfor.text = relicDatas.description;
        int rarityIndex = 0;

        this.relicIcon.sprite = null;
        rarityImage.color= rarityColor[rarityIndex];

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
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectRelic();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rect.sizeDelta = new Vector2(rect.sizeDelta.x * 1.5f,rect.sizeDelta.y* 1.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rect.sizeDelta = new Vector2(rect.sizeDelta.x * 1f,rect.sizeDelta.y* 1f);
    }
}
