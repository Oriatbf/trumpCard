using System;
using UnityEngine;
using UnityEngine.UI;

public class JoyStickController : SingletonDontDestroyOnLoad<JoyStickController>
{
    public VariableJoystick moveJoyStick, dirJoyStick;
    public Button interactionBtn;
    [SerializeField]private PlayableNpcManager playableNpcManager;
    [SerializeField] private Panel panel;

    private void Start()
    {
       
    }

    public void Open()
    {
        if(DataManager.Inst.Data.moblieVersion)
            panel.SetPosition(PanelStates.Show,true);
    }
    
    public void Close()
    {
        if(DataManager.Inst.Data.moblieVersion)
            panel.SetPosition(PanelStates.Hide,true);
    }
}
