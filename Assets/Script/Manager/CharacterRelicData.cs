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
        playerRelicData.Add(relicData);
        DataManager.Inst.Save();
    }

    public void LoadPlayerRelic(List<int> relicIds)
    {
        playerRelicData.Clear();
        foreach (var id in relicIds)
        {
            RelicDatas data = RelicDataManager.Inst.relicDatas.FirstOrDefault(r => r.id == id);
            playerRelicData.Add(data);
        }
    }
}
