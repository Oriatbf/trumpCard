using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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
            relicDatas = RelicDataManager.Inst.GetRelics(data.relicId);
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

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => RelicDataManager.Inst);
        foreach (var data in NpcData.Data.DataList)
        {
            npcDatas.Add(new Data(data));
        }
    }

    public void RandomEnemy(int stage)
    {
        int _difficulty = 0;
        switch (stage)
        {
            case <=3:
                _difficulty = 1;
                break;
            case <=7:
                _difficulty = 2;
                break;
            case <=12:
                _difficulty = 3;
                break;
        }

        List<Data> npcs = npcDatas.Where(n => n.difficulty == _difficulty).ToList();
        int random = Random.Range(0, npcs.Count);
        //리턴
    }
}
