using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TopUIController : MonoBehaviour
{
    private static TopUIController _inst;

    public static TopUIController Inst
    {
        get { return _inst; }
    }
    
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Transform relicContent;
    [SerializeField] private Canvas uiTopBar;
    private RelicIcon relicIcon;
    private int _gold = 0;

    private void Awake()
    {
        if (Inst != this && Inst != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            _inst = this;
            DontDestroyOnLoad(gameObject);
        }
        
        if(relicIcon ==null)
            relicIcon = Resources.Load<RelicIcon>("UIPrefab/Icon");
        UpdateGold();
    }

    private void Start()
    {
        Application.targetFrameRate = 60;  //모바일 최적화
    }
    


    public void Initialize()
    {
        foreach (Transform child in relicContent)
        {
            Destroy(child.gameObject);
        }
        SetGold(DataManager.Inst.Data.gold);
        
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
        if(_relicData.relic.excuteType == ExcuteType.Once)
            _relicData.relic.Excute();

        RelicIcon icon =  Instantiate(relicIcon, relicContent);
        icon.Init(_relicData);
        if(dataSave)
            CharacterRelicData.Inst.AddPlayerRelic(_relicData);
    }


    public void SetGold(int value)
    {
        _gold = value;
        UpdateGold();
    }
    public void GetGold(int value)
    {
        _gold += value;
        UpdateGold();
    }

    private void UpdateGold()
    {
        _gold = _gold < 0 ? 0 : _gold;
        goldText.text = "<sprite=0> " + _gold.ToString();
    }

    public int CurrentGold() => _gold;
}
