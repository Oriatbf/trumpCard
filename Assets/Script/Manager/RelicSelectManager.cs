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
    [SerializeField] private int cardCount;
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

        OnClose +=()=> Hide();
    }
    
    

    [Button]
    public void CardSelect(string sceneName = null)
    {
        panel.SetPosition(PanelStates.Show,true);
        var randomRelics = RelicDataManager.Inst.GetRandomRelics(cardCount);
        foreach (var relicData in randomRelics)
        {
            Card card =  Instantiate(cardPrefab, content);
            card.Init(relicData);
        }

        if (sceneName!= null)
        {
            OnClose += ()=>DemoLoadScene.Inst.LoadScene(sceneName);
        }
        else
        {
            OnClose = null;
            OnClose +=()=> Hide();
        }
    }
    
    private void Hide()
    {
        TimeManager.Inst.ChangeTimeSpeed(1f);
        panel.SetPosition(PanelStates.Hide,true);
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
       
    }

    public void Close()
    {
        OnClose?.Invoke();
        DataManager.Inst.Save();
    
    }
    
    
    
    
}
