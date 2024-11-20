using System;
using UnityEngine;
using EasyTransition;
public class LobbyStartBtn : LobbyInteraction
{
    private void Update()
    {
        DetectPlayer();
    }

    protected override void InteractAction()
    {
        DemoLoadScene.Inst.LoadScene("Map");
    }
}
