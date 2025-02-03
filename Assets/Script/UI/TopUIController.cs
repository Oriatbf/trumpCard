using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TopUIController : SingletonDontDestroyOnLoad<TopUIController>
{

    
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Transform relicContent;
    [SerializeField] private Canvas uiTopBar;
    private RelicIcon relicIcon;

    private int _gold
    {
        get => DataManager.Inst.Data.gold;
    }

    protected void Awake()
    {
        base.Awake();
        
        if(relicIcon ==null)
            relicIcon = Resources.Load<RelicIcon>("UIPrefab/Icon");
      
    }

    private void Start()
    {
        UpdateGold();
        Application.targetFrameRate = 60;  //모바일 최적화
    }
    


    public void Initialize()
    {
        foreach (Transform child in relicContent)
        {
            Destroy(child.gameObject);
        }
        UpdateGold();
    }
    
    public void InstanceRelicIcon(object ids,bool dataSave) // DataManager에서 로드될때는 false
    {
        List<RelicDatas> relicDatas =  RelicDataManager.Inst.GetRelics(ids);
        foreach (var data in relicDatas)
        {
            InstanceRelicIcon(data,dataSave);
        }
    }
    public void InstanceRelicIcon(RelicDatas _relicData,bool dataSave)//세이브해야 하는지
    {
        if (relicIcon == null)
        {
            Debug.LogError("RelicIconPrefab is Null");
            return;
        }
        if(_relicData.relic.excuteType == ExcuteType.OnGet)
            _relicData.relic.Excute();

        RelicIcon icon =  Instantiate(relicIcon, relicContent);
        icon.Init(_relicData);
        if(dataSave)
            CharacterRelicData.Inst.AddPlayerRelic(_relicData);
    }



    public void GetGold(int value)
    {
        ReCountGold(_gold,value);
    }
    
    
    private void ReCountGold(int curGold,int value)
    {
        DOTween.To(() => curGold, x =>
        {
            if (x >= 0)
            {
                curGold = x;
                goldText.text = $"<sprite=0> {x}";
            }
           
        }, curGold + value, 0.5f);
        DataManager.Inst.Data.gold+=value;
        UpdateGold();
    }


    private void UpdateGold()
    {
        DataManager.Inst.Data.gold = DataManager.Inst.Data.gold < 0 ? 0 : DataManager.Inst.Data.gold;
        goldText.text = "<sprite=0> " + _gold.ToString();
    }

    public int CurrentGold() => _gold;
}
