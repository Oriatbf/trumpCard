using System;
using System.Collections.Generic;
using EasyTransition;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using VInspector;
using Object = System.Object;
using Random = UnityEngine.Random;

public class RelicSelectController : SingletonDontDestroyOnLoad<RelicSelectController>
{
    [SerializeField] private Panel panel;
    [SerializeField]private int curRepeat = 1;

    [SerializeField] private int cardCount;

    private int cardRepeat;
  
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Transform content;

    private Action OnClose;

    protected override void Awake()
    {
        base.Awake();
    }

    private void AdjustContentScale(int _cardCount)
    {
        if (_cardCount > 4)
        {
            float size = 1 - (((cardCount-4)*0.1f)+0.1f);
            content.localScale = new Vector3(size, size);
        }
    }

    [Button]
    public void CardSelect(string sceneName = null)
    {
        cardCount = DataManager.Inst.Data.cardCount;
        cardRepeat = DataManager.Inst.Data.cardRepeat;
        
        AdjustContentScale(cardCount);
        
        if(curRepeat<= cardRepeat)
        {
            ShowCards(sceneName);
        }
        else
        {
            FinalizeSelection(sceneName);
        }
    }

    private void ShowCards(string sceneName)
    {
        DestroyChild();
        panel.SetPosition(PanelStates.Show,true);
        
        var randomRelics = RelicDataManager.Inst.GetRandomRelics(cardCount);
        foreach (var relicData in randomRelics)
        {
            Card card =  Instantiate(cardPrefab, content);
            card.Init(relicData);
        }
        
        OnClose = ()=>CardSelect(sceneName);
        curRepeat++;
    }

    private void FinalizeSelection(string sceneName)
    {
        OnClose = null;
        OnClose += ()=>curRepeat = 1;
        if (sceneName!= null)
        {
            OnClose += ()=>DemoLoadScene.Inst.LoadScene(sceneName);
        }
        OnClose +=()=> Hide();
        OnClose?.Invoke();
    }
    
    private void Hide()
    {
        TimeManager.Inst.ChangeTimeSpeed(1f);
        panel.SetPosition(PanelStates.Hide,true);
        DestroyChild();
        
       
    }

    private void DestroyChild()
    {
        if (content.childCount > 0)
        {
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }
        }
      
    }

    public void Close()
    {
        OnClose?.Invoke();
        DataManager.Inst.Save();
    }
    
    public Card InstanceCard(RelicDatas relicData,Transform trans,Vector2 scale)
    {
        Card card =  Instantiate(cardPrefab, trans);
        card.Init(relicData,scale,false);
        return card;
    }
    
    
    
    
}
