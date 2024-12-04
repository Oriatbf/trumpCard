using System;
using UnityEngine;

public class RelicBase
{
    public float value;

    public virtual void Init(float _value)
    {
        value = _value;
    }

    public virtual void Excute(Character character){}
    
}