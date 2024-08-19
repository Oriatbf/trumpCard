using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Inst;

    public bool  isInGame; //게임 중 인지 확인

    public TextMeshProUGUI goldText;
    public float gold;

    private void Awake()
    {
        Inst = this;
        isInGame= true;
    }
    // Start is called before the first frame update
    void Start()
    {
        GoldCount(0);
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
