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
        GoldCount(0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InstanceRelicIcon(RelicSO relicSO)
    {
        GameObject icon =  Instantiate(relicIcon, relicIconCanvas);
        icon.GetComponent<Image>().sprite = relicSO.relicIcon;
    }



    public void GoldCount(float gold)
    {
        this.gold += gold;
        goldText.text = "<sprite=0> " + this.gold.ToString();
    }
}
