using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GambleManager : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerTypeManager.Inst.TypeChange(Gambling());
        }
    }

    public int Gambling()
    {
        int a = Random.Range(0,12);
        return a;
    }
}
