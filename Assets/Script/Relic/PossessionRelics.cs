using System;
using System.Collections.Generic;
using UnityEngine;

public class PossessionRelics : MonoBehaviour
{
    Character character;
    public List<RelicBase> possessionRelics = new List<RelicBase>();
    public string str;
    
    private void Awake()
    {
        character = GetComponent<Character>();
        Type type = Type.GetType(str);
        if (type != null)
        {
            object relicInstance = Activator.CreateInstance(type);
            RelicBase relic = relicInstance as RelicBase;
            relic.Init(8.5f);
            possessionRelics.Add(relic);
        }
        
       
    }

    private void Start()
    {
        foreach (var relic in possessionRelics)
        {
            relic.Excute(character);
        }
    }
}
