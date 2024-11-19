using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class UIManager : MonoBehaviour
{
    public static UIManager Inst;

    public TextMeshProUGUI goldText;
    [SerializeField] GameObject relicIcon;
    [SerializeField] Transform relicIconCanvas;
    [SerializeField] Canvas uiTopBar;
 
   
    public float gold;

    [Foldout("Inchant")]
    [SerializeField] CardStats[] numberCards;
    [SerializeField] Image[] inchantCardImage;
    [SerializeField] Canvas inchantCanvas;
    Animator inchantAnim;



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


    // Start is called before the first frame update
    void Start()
    {
        inchantAnim= inchantCanvas.GetComponent<Animator>();
        Application.targetFrameRate = 60;  //모바일 최적화
        gold = 0;
        GoldCount(0);
    }

    public void GoldRelicReset()
    {
        Debug.Log("리셋");
        uiTopBar.enabled = true;
        gold = 0;
        GoldCount(0);
        for(int i = 0;i<relicIconCanvas.childCount;i++)
        {
            Destroy(relicIconCanvas.GetChild(i).gameObject);
        }
    }

    public void EndingScene()
    {
        uiTopBar.enabled= false;
    }


    public void InstanceRelicIcon(RelicSO relicSO)
    {
        GameObject icon =  Instantiate(relicIcon, relicIconCanvas);
        icon.GetComponent<Image>().sprite = relicSO.relicIcon;
        icon.transform.Find("RelicLootImage").GetComponent<RelicLoot>().SetCard(relicSO);
    }

    public void InchantCanvasAnim(List<int> inchantList,Sprite inchantIcon)
    {
        for(int i = 0; i<inchantList.Count;i++)
        {
            for(int j = 0; j < 13; j++)
            {
                if (inchantList[i] == j)
                {
                    inchantCardImage[i].sprite = numberCards[j].infor.playerCardImage;
                    inchantCardImage[i].transform.GetChild(0).GetComponent<Image>().sprite = inchantIcon;
                }
            }
        }
        inchantAnim.SetTrigger("Intro");
        
    }



    public void GoldCount(float gold)
    {
        this.gold += gold;
        goldText.text = "<sprite=0> " + this.gold.ToString();
    }
}
