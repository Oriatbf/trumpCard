using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Inst;
    
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private GameObject relicIcon;
    [SerializeField] private Transform relicIconCanvas;
    [SerializeField] private Canvas uiTopBar;

    

    private int _gold = 0;

    private void Awake()
    {
        Inst = this;
    }

    private void Start()
    {
        Application.targetFrameRate = 60;  //모바일 최적화
    }

    public void InstanceRelicIcon(RelicSO relicSO)
    {
        GameObject icon =  Instantiate(relicIcon, relicIconCanvas);
        icon.GetComponent<Image>().sprite = relicSO.relicIcon;
       // icon.transform.Find("RelicLootImage").GetComponent<Card>().SetCard(relicSO); //수정 예정
    }

    public void GetGold(int value)
    {
        _gold += value;
  
        ShowGold();
    }

    private void ShowGold()
    {
        _gold = _gold < 0 ? 0 : _gold;
        goldText.text = "<sprite=0> " + _gold.ToString();
    }

    public void ResetResources()
    {
        foreach (Transform _child in relicIconCanvas)
        {
           Destroy(_child.gameObject);
        }

        _gold = 0;
        ShowGold();
    }

    public int CurrentGold() => _gold;
}
