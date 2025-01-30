using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PossessionRelics : MonoBehaviour
{
    Character character;
    private Health health;
    public List<RelicBase> possessionRelics = new List<RelicBase>();
    
    private void Awake()
    {
        health = GetComponent<Health>();
        character = GetComponent<Character>();
        StartCoroutine(SetRelic());
        if (health.characterType == CharacterType.Player)
        {
            foreach (var relicData in CharacterRelicData.Inst.playerRelicData)
            {
                possessionRelics.Add(relicData.relic);
            }
        }
        else if (health.characterType == CharacterType.Enemy)
        {
            foreach (var relicData in  GameManager.Inst.enemyData.relicDatas)
            {
                possessionRelics.Add(relicData.relic);
            }
        }
       
        
    }

    public void ExcuteRelic()
    {
        foreach (var relic in possessionRelics)
        {
            relic.Excute(character);
            StartCoroutine(relic.ExcuteCor(character));
        }
    }

    IEnumerator  SetRelic()
    {
        yield return new WaitUntil(() => GameManager.Inst);
      
       
    }

    private void Start()
    {
        
    }
}
