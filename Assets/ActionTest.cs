using System;
using UnityEngine;

public class ActionTest : MonoBehaviour
{
    public Action action;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        action += () => Debug.Log("ppp");
        
        action?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
