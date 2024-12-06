using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class RelicSelectManager : MonoBehaviour
{
    public static RelicSelectManager Inst;
    [SerializeField] private int cardCount;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Transform content;

    private void Awake()
    {
        Inst = this;
    }

    public void CardSelect()
    {

        HashSet<int> selectedId = new HashSet<int>();
        
        for (int j = 0; j < cardCount; j++)
        {
            int random = 0;
            do
            {
                 random = Random.Range(0, RelicDataManager.Inst.relicDatas.Count);
            } while (!selectedId.Add(random));
        }
        for (int i = 0; i < cardCount; i++)
        {
            Card card =  Instantiate(cardPrefab, content);
           // card.Init
        }
    }
    
    
    
    
}
