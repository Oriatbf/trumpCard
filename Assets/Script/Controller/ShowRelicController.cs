using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class ShowRelicController : SingletonDontDestroyOnLoad<ShowRelicController>
{
    [SerializeField] private Panel panel;
    [SerializeField] private Card card;
    private Action onShow, onClose;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        onShow += ()=>panel.SetPosition(PanelStates.Show, true);
        onClose += () => panel.SetPosition(PanelStates.Hide, true);
    }

    public void Show(List<RelicDatas> _relicDatas)
    {
        StartCoroutine(ShowRelicInfo(_relicDatas));
    }

  
    private IEnumerator ShowRelicInfo(List<RelicDatas> _relicDatas)
    {
        for (int i = 0; i < _relicDatas.Count; i++)
        {
            card.Init(_relicDatas[i]);
            onShow?.Invoke();
            yield return new WaitForSeconds(0.7f);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            onClose?.Invoke();
            yield return new WaitForSeconds(0.5f);
        }
    }
}
