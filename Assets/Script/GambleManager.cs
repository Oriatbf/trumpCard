using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GambleManager : MonoBehaviour
{
    private void Start()
    {
        PlayerTypeManager.Inst.TypeChange(Gambling());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && UIManager.Inst.gambleAllow)
        {
            PlayerTypeManager.Inst.TypeChange(Gambling());
        }
    }

    public int Gambling()
    {
        UIManager.Inst.gambleAllow = false;
        int a = Random.Range(0,12);
        UIManager.Inst.EndGamble();
        return a;
    }

}
