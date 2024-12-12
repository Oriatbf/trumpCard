using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PossessionRelics : MonoBehaviour
{
    Character character;
    public List<RelicBase> possessionRelics = new List<RelicBase>();
    
    private void Awake()
    {
        character = GetComponent<Character>();
        SetRelic();
        
        foreach (var relic in possessionRelics)
        {
            relic.Excute(character);
            StartCoroutine(relic.ExcuteCor(character));
        }
        
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
        else if (character.characterType == CharacterType.Enemy)
        {
            int random = Random.Range(0, NpcDataManager.Inst.npcDatas.Count);
            foreach (var relicData in  NpcDataManager.Inst.npcDatas[random].relicDatas)
            {
                possessionRelics.Add(relicData.relic);
            }
        }
       
    }

    private void Start()
    {
        
    }
}
