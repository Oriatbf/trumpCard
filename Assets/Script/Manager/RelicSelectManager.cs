using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using VInspector;
using Object = System.Object;
using Random = UnityEngine.Random;

public class RelicSelectManager : MonoBehaviour
{
    public static RelicSelectManager Inst;
    [SerializeField] private Panel panel;
    [SerializeField] private int cardCount;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Transform content;

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
    }

    [Button]
    public void CardSelect()
    {
        panel.SetPosition(PanelStates.Show,true);
        var randomRelics = RelicDataManager.Inst.RandomRelics(cardCount);
        foreach (var relicData in randomRelics)
        {
            Card card =  Instantiate(cardPrefab, content);
            card.Init(relicData);
        }
    }

    public void Close()
    {
        panel.SetPosition(PanelStates.Hide,true);
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }
    
    
    
    
}
