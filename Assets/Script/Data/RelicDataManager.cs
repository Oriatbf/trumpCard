using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using GoogleSheet.Core.Type;
using UnityEngine;
using UnityEngine.Serialization;
using VHierarchy.Libs;
using Random = UnityEngine.Random;

[UGS(typeof(Rarity))]
public enum Rarity
{
    Common,Rare,Epic
}

[Serializable]
public class RelicDatas
{
    public Rarity rarity;
    public string name;
    public string description;
    public Dictionary<string, string> descriptionVariable = new Dictionary<string, string>();
    public int id;
    public List<int> extraRelicID = new List<int>();
    public RelicBase relic;

    public RelicDatas(RelicData.Data data,RelicBase relic,List<int> exrtaRelicID)
    {
        this.relic = relic;
        this.extraRelicID = exrtaRelicID;
        rarity = data.rarity;
        id = data.id;
        name = data.name;
        descriptionVariable.Add("time", data.time.ToString());
        descriptionVariable.Add("value", data.value.ToString());
        description = data.description;
        foreach (var word in descriptionVariable)
        {
            description = description.Replace(word.Key,word.Value);
        }

        Debug.Log(description);
    }

    public RelicDatas(RelicDatas originRelic,RelicBase relicClone)
    {
        extraRelicID = originRelic.extraRelicID;
        rarity = originRelic.rarity;
        id = originRelic.id;
        name = originRelic.name;
        description = originRelic.description;
        relic = relicClone;

    }
    
        
}

public class RelicDataManager : MonoBehaviour
{
    public static RelicDataManager Inst;
    public List<RelicDatas> relicDatas  = new List<RelicDatas>();
   
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
                relic.Init(data.value,data.time);
                
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
                    
                    data.relic.extraRelic.Add(extraRelic.relic);
                }
            }
        }
    }

    public List<RelicDatas> GetRandomRelics(int count)
    {
        HashSet<int> selectedID = new HashSet<int>();
        HashSet<RelicDatas> selectedRelic = new HashSet<RelicDatas>();
        
        
        
        for (int j = 0; j < count; j++)
        {
            int random = 0;
            do
            {
                random = Random.Range(0, relicDatas.Count);
            } while (!selectedID.Add(random));

            var originalRelic = relicDatas.FirstOrDefault(r => r.id == random);
            var copyRelic = new RelicDatas(originalRelic, originalRelic.relic.Clone());
            selectedRelic.Add(copyRelic);
        }

        return selectedRelic.ToList();
    }

    public List<RelicDatas> GetIdRelics(string[] ids)
    {
        List<int> relicIds = new List<int>();
        foreach (var id in ids)
        {
            if(int.TryParse(id,out int relicId))
                relicIds.Add(relicId);
        }
       
        List<RelicDatas> selectedRelic = new List<RelicDatas>();
        foreach (var id in relicIds)
        {
            var originalRelic = relicDatas.FirstOrDefault(r => r.id == id);
            var copyRelic = new RelicDatas(
                new RelicData.Data
                {
                    id = originalRelic.id,
                    name = originalRelic.name,
                    description = originalRelic.description,
                    rarity = originalRelic.rarity,
                    time = float.Parse(originalRelic.descriptionVariable["time"]),
                    value = float.Parse(originalRelic.descriptionVariable["value"]),
                    extraRelicID = string.Join(",", originalRelic.extraRelicID),
                },
                originalRelic.relic.Clone(),
                originalRelic.extraRelicID
            );
            selectedRelic.Add(copyRelic);
           
        }
        return selectedRelic.ToList();
    }
     
    public List<RelicDatas> GetDupRandomRelics(int count)
    {
        List<int> selectedID = new List<int>();
        List<RelicDatas> selectedRelic = new List<RelicDatas>();
        
        for (int j = 0; j < count; j++)
        {
            int random = 0;
            random = Random.Range(0, relicDatas.Count);
            var originalRelic = relicDatas.FirstOrDefault(r => r.id == random);
            var copyRelic = originalRelic;
            selectedRelic.Add(copyRelic);
        }

        return selectedRelic.ToList();
    }
}
