using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Inst;

    public TextMeshProUGUI goldText;
    [SerializeField] GameObject relicIcon;
    [SerializeField] Transform relicIconCanvas;
   
    public float gold;


    private void Awake()
    {
        if (Inst != this && Inst != null)
        {
            Destroy(transform.gameObject);
            
            return;
        }
        else
        {
            Inst = this;
            DontDestroyOnLoad(transform.gameObject);
        }
    }

   

    public void GameStart()
    {
      
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("스타토");
        gold = 0;
        GoldCount(0);
    }

    public void GoldRelicReset()
    {
        Debug.Log("리셋");
        gold= 0;
        GoldCount(0);
        for(int i = 0;i<relicIconCanvas.childCount;i++)
        {
            Destroy(relicIconCanvas.GetChild(i).gameObject);
        }
    }


    public void InstanceRelicIcon(RelicSO relicSO)
    {
        GameObject icon =  Instantiate(relicIcon, relicIconCanvas);
        icon.GetComponent<Image>().sprite = relicSO.relicIcon;
        icon.transform.Find("RelicLootImage").GetComponent<RelicLoot>().SetCard(relicSO);
    }



    public void GoldCount(float gold)
    {
        this.gold += gold;
        goldText.text = "<sprite=0> " + this.gold.ToString();
    }
}
