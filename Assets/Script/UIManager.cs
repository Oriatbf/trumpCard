using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Inst;

    public TextMeshProUGUI goldText;
    public float gold;

    private void Awake()
    {
        if (Inst != this && Inst != null)
        {
            return;
        }
        else
        {
            Inst = this;
        }
    }

    public void GameStart()
    {
      
    }
    // Start is called before the first frame update
    void Start()
    {
        ;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EndGamble()
    {
 
    }



    public void GoldCount(float gold)
    {
        this.gold += gold;
        goldText.text = "골드 : " + this.gold.ToString();
    }
}
