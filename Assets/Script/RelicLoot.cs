using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class RelicLoot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI relicName,relicInfor;
    [SerializeField] Color[] rarityColor;
    [SerializeField] Image rarityImage,relicIcon;
    RelicSO curRelic;

    // Shop
    [SerializeField] bool ShopCard;

    [ShowIf("ShopCard")]
    [SerializeField] TextMeshProUGUI goldText;

    public bool purchased;
    private int gold;

    public void SetCard(RelicSO relicSO)
    {
        curRelic= relicSO;
        relicName.text = relicSO.relicName;
        relicInfor.text = relicSO.relicInfor;
        int rarityIndex = 0;
        switch (relicSO.rarity)
        {
            case RelicSO.Rarity.Common:  rarityIndex = 0; break;
            case RelicSO.Rarity.Rare: rarityIndex = 1; break;
            case RelicSO.Rarity.Epic: rarityIndex = 2; break;
        }
        this.relicIcon.sprite = relicSO.relicIcon;
        rarityImage.color= rarityColor[rarityIndex];

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
        }
    }

    public void SelectRelic()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<RelicSkills>().relics.Add(curRelic);
        player.GetComponent<Character>().StartRelicSkill();
        UIManager.Inst.InstanceRelicIcon(curRelic);
        if (!GameManager.Inst.isGameEnd) GameManager.Inst.GameStart();
        else
        {
            Time.timeScale = 1.0f;
            GameManager.Inst.mapMode = true;
            GameManager.Inst.SceneTransition("RealMap");
   
        }
    }

    // Shop
    public void BuyRelic()
    {
        if(!purchased && UIManager.Inst.gold >= gold)
        {
            purchased = true;

            UIManager.Inst.GoldCount(-gold);
            goldText.text = "SALE";

            // Shop

            RelicManager.Inst.playerRelic.Add(curRelic);
            UIManager.Inst.InstanceRelicIcon(curRelic);
        }
    }
}
