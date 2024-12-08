using System;
using System.Collections;
using System.Collections.Generic;
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
        for (int i = 0; i < 10; i++)
        {
            enemyRelicData.Add(RelicDataManager.Inst.DupRandomRelics(i + 1));
        }
    }
    
    public void AddPlayerRelic(RelicDatas relicData)
    {
        playerRelicData.Add(relicData);
    }
}
