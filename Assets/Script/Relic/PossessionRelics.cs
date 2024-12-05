using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PossessionRelics : MonoBehaviour
{
    Character character;
    public List<RelicBase> possessionRelics = new List<RelicBase>();
    public string str;
    
    private void Awake()
    {
        character = GetComponent<Character>();
        var relic = RelicDataManager.Inst.relicDatas.FirstOrDefault(r => r.name == str);
        possessionRelics.Add(relic.relic);
        
       
    }

    private void Start()
    {
        foreach (var relic in possessionRelics)
        {
            relic.Excute(character);
        }
    }
}
