using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using VHierarchy.Libs;
using VInspector.Libs;

public class RelicDataManager : MonoBehaviour
{
    public static RelicDataManager Inst;
    public List<RelicDatas> relicDatas  = new List<RelicDatas>();
    [Serializable]
    public class RelicDatas
    {
        public string name;
        public string description;
        public int id;
        public List<int> extraRelicID = new List<int>();
        public RelicBase relic;

        public RelicDatas(RelicData.Data data,RelicBase relic,List<int> exrtaRelicID)
        {
            this.relic = relic;
            this.extraRelicID = exrtaRelicID;
            id = data.id;
            name = data.name;
            description = data.description;
        }

        public RelicDatas(RelicDatas relicDatas)
        {
            this.relic = relicDatas.relic;
            this.extraRelicID = relicDatas.extraRelicID;
            id = relicDatas.id;
            name = relicDatas.name;
            description = relicDatas.description;
        }
    }
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
        RelicData.Data.Load();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var data in RelicData.Data.DataList)
        {
            Type type = Type.GetType(data.component);
            if (type != null)
            {
                object relicInstance = Activator.CreateInstance(type);
                RelicBase relic = relicInstance as RelicBase;
                List<int> _extraRelicID = new List<int>();
                if (data.extraRelicID != "")
                {
                    string[] extraRelics = data.extraRelicID.Split(",");
                    foreach (var str in extraRelics)
                    {
                        _extraRelicID.Add(int.Parse(str));
                    }
                }
                relic.Init(data.value);
                
                relicDatas.Add(new RelicDatas( data,relic,_extraRelicID));
            }
        }

        foreach (var data in relicDatas)
        {
            if (data.extraRelicID.Count>0)
            {
                //string[] _extraRelicIds = data.extraRelicID
                foreach (var relicIndex in data.extraRelicID)
                {
                    var extraRelic =  relicDatas.FirstOrDefault(r => r.id == relicIndex);
                    
                    var a = new RelicDatas(extraRelic);
                    data.relic.extraRelic.Add(a.relic);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
