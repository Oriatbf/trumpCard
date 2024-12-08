using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class PossessionRelics : MonoBehaviour
{
    Character character;
    public List<RelicBase> possessionRelics = new List<RelicBase>();
    [FormerlySerializedAs("relicid")] public List<int> relicids = new List<int>();
    
    private void Awake()
    {
        character = GetComponent<Character>();
        SetRelic();
        
    }

    private void SetRelic()
    {
        if (character.characterType == CharacterType.Player)
        {
            foreach (var relicData in CharacterRelicData.Inst.playerRelicData)
            {
                possessionRelics.Add(relicData.relic);
            }
        }
       
    }

    private void Start()
    {
        foreach (var relic in possessionRelics)
        {
            relic.Excute(character);
            StartCoroutine(relic.ExcuteCor(character));
        }
    }
}
