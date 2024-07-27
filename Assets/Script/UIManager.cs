using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Inst;
    public Image gambleGauge;
    public bool gambleAllow, isInGame; //겜블 가능한지 , 게임 중 인지 확인
    public float gamebleGauagePerSecond;

    private void Awake()
    {
        Inst = this;
        isInGame= true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isInGame && !gambleAllow)
        {
            gambleGauge.fillAmount += gamebleGauagePerSecond * Time.deltaTime;
        }

        if(gambleGauge.fillAmount >= 1)
        {
            gambleAllow= true;
        }
    }

    public void EndGamble()
    {
        gambleAllow= false;
        gambleGauge.fillAmount = 0;

    }
}
