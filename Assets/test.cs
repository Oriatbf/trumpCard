using System;
using System.Collections;
using UnityEngine;

public class test : MonoBehaviour
{
    public string str;

    private void Start()
    {
        Type type = Type.GetType(str);
        if (type != null)
        {
            object relicInstance = Activator.CreateInstance(type);
            RelicBase relic = relicInstance as RelicBase;
            relic.Init(8.5f);
            Debug.Log(relic.value);
        }
    }
}
