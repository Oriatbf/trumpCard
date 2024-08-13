using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RelicLoot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI relicName,relicInfor;
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
    }

    public void SelectRelic()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<RelicSkills>().relics.Add(curRelic);
    }

}
