using System;
using UnityEngine;
using EasyTransition;
using UnityEngine.UI;

public class LobbyStartBtn : LobbyInteraction
{
    [SerializeField] private Panel panel;
    [SerializeField] private Image ui;

    [SerializeField] private float offsetY;

    private void Update()
    {
        DetectPlayer();
        
    }

    protected override void NearAction()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0,offsetY));
        ui.transform.position = pos;
        panel.SetPosition(PanelStates.Show,true);
    }

    protected override void DetectOutPlayer()
    {
        panel.SetPosition(PanelStates.Hide,true);
    }

    protected override void InteractAction()
    {
        DemoLoadScene.Inst.LoadScene("Map");
    }
}
