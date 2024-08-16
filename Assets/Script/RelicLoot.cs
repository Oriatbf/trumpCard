using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RelicLoot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI relicName,relicInfor;
    [SerializeField] Color[] rarityColor;
    [SerializeField] Image rarityImage;
    RelicSO curRelic;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

        rarityImage.color= rarityColor[rarityIndex];
    }

    public void SelectRelic()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<RelicSkills>().relics.Add(curRelic);
        if (!GameManager.Inst.isGameEnd) GameManager.Inst.GameStart();
    }

}
