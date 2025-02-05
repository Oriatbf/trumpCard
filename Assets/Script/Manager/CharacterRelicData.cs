using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterRelicData : MonoBehaviour
{
    public static CharacterRelicData Inst;
    
    public List<RelicDatas> playerRelicData = new List<RelicDatas>();
    public List<List<RelicDatas>> enemyRelicData = new List<List<RelicDatas>>();

    private void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Inst = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    

    private IEnumerator Start()
    {
        yield return new WaitUntil(()=>RelicDataManager.Inst);
    }
    
    public void AddPlayerRelic(RelicDatas relicData)
    {
        DataManager.Inst.Data.relicID.Add(relicData.id);
        RelicDuplication(relicData);
        DataManager.Inst.Save();
    }

    void RelicDuplication(RelicDatas relicData)
    {
        var relic =  playerRelicData.FirstOrDefault(d => d.id == relicData.id);
        if (relic != null)
        {
            relic.relic.value += relicData.relic.value;
            Debug.Log("똑같은거 얻음");
        }
        else
        {
            playerRelicData.Add(relicData);
        }
    }

    public void LoadPlayerRelic(List<int> relicIds)
    {
        playerRelicData.Clear();
        foreach (var id in relicIds)
        {
            RelicDatas data = RelicDataManager.Inst.relicDatas.FirstOrDefault(r => r.id == id);
            RelicDuplication(data);
        }
    }
}
