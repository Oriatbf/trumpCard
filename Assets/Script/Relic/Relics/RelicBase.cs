using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class RelicBase
{
    
    public float value;
    public float time;
    public List<RelicBase> extraRelic = new List<RelicBase>();
    


    public virtual void Init(float _value,float _time)
    {
        value = _value;
        time = _time;
    }

    public virtual void Excute(Character character)
    {
        if (extraRelic.Count>0)
        {
            foreach (var _extraRelic in extraRelic)
            {
                _extraRelic.Excute(character);
            }
        }
    }

    public virtual IEnumerator ExcuteCor(Character character)
    {
        yield return null;
    }
    
}