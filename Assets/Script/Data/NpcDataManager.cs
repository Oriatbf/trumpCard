using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class NpcDataManager : MonoBehaviour
{
    [Serializable]
    public class Data
    {
        public int id;
        public string name;
        public int difficulty;
        public string description;
        public List<RelicDatas> relicDatas = new List<RelicDatas>();

        public Data(NpcData.Data data)
        {
            id = data.id;
            name = data.name;
            difficulty = data.difficulty;
            description = data.description;
            string[] idString = data.relicId.Split(",");
            relicDatas = RelicDataManager.Inst.GetIdRelics(idString);
        }

    }
    
    public static NpcDataManager Inst;
    public List<Data> npcDatas = new List<Data>();
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
        NpcData.Data.Load();
    }

    private void Start()
    {
        foreach (var data in NpcData.Data.DataList)
        {
            npcDatas.Add(new Data(data));
        }
    }
}
