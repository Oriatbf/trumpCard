using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using GoogleSheet.Core.Type;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D;
using VHierarchy.Libs;
using Random = UnityEngine.Random;

[UGS(typeof(Rarity))]
public enum Rarity
{
    Common=0,Rare=1,Epic=2,Nothing = -1
}



[Serializable]
public class RelicDatas
{
    public Rarity rarity;
    public string name;
    public string description;
    public Sprite sprite;
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
        Sprite[] sprites = Resources.LoadAll<Sprite>("RelicIcon/SKills");
        sprite = sprites.FirstOrDefault(s => s.name == data.sprite);
        if (sprite == null) Debug.Log("스프라이트 없음");
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
        sprite = originRelic.sprite;
        relic = relicClone;

    }
    
        
}

public class RelicDataManager : MonoBehaviour
{
    public static RelicDataManager Inst;
    public List<RelicDatas> relicDatas  = new List<RelicDatas>();
    private List<RelicDatas> InActiveRelicDatas = new List<RelicDatas>();
   
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
            
            RelicBase relic = null;
            Type type = Type.GetType(data.component);
            if (type != null)
            {
                object relicInstance = Activator.CreateInstance(type);
                relic = relicInstance as RelicBase;
            }

            List<int> _extraRelicID = new List<int>();
                
            if (data.extraRelicID != "") //추가 유물 아이디 string -> int로 변환
            {
                string[] extraRelics = data.extraRelicID.Split(",");
                foreach (var str in extraRelics)
                {
                    _extraRelicID.Add(int.Parse(str));
                }
            }
            
            relic?.Init(data.value,data.time,data.excuteType);
            
            InActiveRelicDatas.Add(new RelicDatas( data,relic,_extraRelicID));
            if(data.active ==1) //1일때 active 0일 때 InActive
                relicDatas.Add(new RelicDatas( data,relic,_extraRelicID));
            
        }

        foreach (var data in relicDatas)
        {
            if (data.extraRelicID.Count>0)
            {
                foreach (var relicIndex in data.extraRelicID)
                {
                    var extraRelic =  InActiveRelicDatas.FirstOrDefault(r => r.id == relicIndex);
                    
                    data.relic.extraRelic.Add(extraRelic.relic);
                }
            }
        }
    }


   

    public List<RelicDatas> GetRelics(object input)
    {
        List<RelicDatas> _relicIds = new List<RelicDatas>();
        switch (input)
        {
            case string ids:
                _relicIds = GetRelics(ids);
                break;
            case List<int> ids:
                _relicIds = GetRelics(ids);
                break;
            default:
                Debug.LogError("자료형이 잘못됨");
                break;
        }

        return _relicIds;
    }
    
    private List<RelicDatas> GetRelics(string _ids)
    {
        string[] ids = _ids.Split(',');
        List<int> relicIds = new List<int>();
        foreach (var id in ids)
        {
            if(int.TryParse(id,out int relicId))
                relicIds.Add(relicId);
        }
        
        return GetRelics(relicIds);
    }
    private List<RelicDatas> GetRelics(List<int> relicIds)
    {
        List<RelicDatas> _selectedRelic = new List<RelicDatas>();
        foreach (var id in relicIds)
        {
            var originalRelic = relicDatas.FirstOrDefault(r => r.id == id);
            var copyRelic = new RelicDatas(originalRelic,originalRelic.relic.Clone());
            if (copyRelic.extraRelicID.Count > 0)
            {
                foreach (var _extraId in copyRelic.extraRelicID)
                {
                    var _originalRelic = InActiveRelicDatas.FirstOrDefault(s => s.id == _extraId);
                    var _copyRelic =  new RelicDatas(_originalRelic,_originalRelic.relic.Clone());
                    copyRelic.relic.extraRelic.Add(_copyRelic.relic);
                }
            }
            _selectedRelic.Add(copyRelic);
           
        }

        return _selectedRelic;
    }
    
    
    public List<RelicDatas> GetRandomRelics(int count)
    {
        HashSet<RelicDatas> originalRelics = new HashSet<RelicDatas>();
        HashSet<RelicDatas> copyRelics = new HashSet<RelicDatas>();
        
        for (int j = 0; j < count; j++)
        {
            int rarityRandom = Random.Range(0, 1000);
            Rarity curRarity = Rarity.Common;
            switch (rarityRandom)
            {
                case <550:
                    curRarity = Rarity.Common;
                    break;
                case <900:
                    curRarity = Rarity.Rare;
                    break;
                case <1000:
                    curRarity = Rarity.Epic;
                    break;
            }

            List<RelicDatas> rarityRelics = relicDatas.Where(r => r.rarity == curRarity).ToList();
            
            int random = 0;
            do
            {
                random = Random.Range(0, rarityRelics.Count);
            } while (!originalRelics.Add(rarityRelics[random]));

            var originalRelic = rarityRelics[random];
            var copyRelic = new RelicDatas(originalRelic, originalRelic.relic.Clone());
            copyRelics.Add(copyRelic);
        }

        return copyRelics.ToList();
    }

    
}
