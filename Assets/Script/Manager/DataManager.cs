using System;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Inst;

    public int stage = 0;
    public int gold = 0;
    
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
}
