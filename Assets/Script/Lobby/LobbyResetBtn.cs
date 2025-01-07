using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyResetBtn : LobbyInteraction
{
    [SerializeField] private Panel panel;
    [SerializeField] private Image ui;

    [SerializeField] private float offsetY;
    protected override void InteractAction()
    {
        DataManager.Inst.ResetData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

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
}
