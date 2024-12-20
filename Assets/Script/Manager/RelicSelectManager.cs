using System;
using System.Collections.Generic;
using EasyTransition;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using VInspector;
using Object = System.Object;
using Random = UnityEngine.Random;

public class RelicSelectManager : MonoBehaviour
{
    public static RelicSelectManager Inst;
    [SerializeField] private Panel panel;
    [SerializeField]private int curRepeat = 1;

    [SerializeField] private int cardCount;

    private int cardRepeat;
  
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Transform content;

    private Action OnClose;

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
    public void CardSelect(string sceneName = null)
    {
        cardCount = DataManager.Inst.Data.cardCount;
        cardRepeat = DataManager.Inst.Data.cardRepeat;
        if (cardCount > 4)
        {
            float size = 1 - (((cardCount-4)*0.1f)+0.1f);
            content.localScale = new Vector3(size, size);
        }
        
        if(curRepeat<= cardRepeat)
        {
            DestroyChild();
            panel.SetPosition(PanelStates.Show,true);
            var randomRelics = RelicDataManager.Inst.GetRandomRelics(cardCount);
            foreach (var relicData in randomRelics)
            {
                Card card =  Instantiate(cardPrefab, content);
                card.Init(relicData);
            }
        
            OnClose = null;
            OnClose += ()=>CardSelect(sceneName);
            ++curRepeat;
        }
        else
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
