using System;
using DG.Tweening;
using EasyTransition;
using UnityEngine;
using UnityEngine.UI;

public class EndCredit : MonoBehaviour
{
    [SerializeField] private Panel skipBtnPanel;
    public Button skipBtn;
    bool pressOn;

    private void Start()
    {
        skipBtn.onClick.AddListener(() => Skip());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !pressOn)
        {
            pressOn = true;
            skipBtnPanel.SetPosition(PanelStates.Show,true);
        }
    }

    public void Skip()
    {
        DataManager.Inst.ResetData();
        DemoLoadScene.Inst.LoadScene("LobbyScene");
    }

    public void SkipCoolTime()
    {
        DOVirtual.DelayedCall(3f, () => Skip());
    }
}
