using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GoogleSheet.Core.Type;
using UnityEngine;

[UGS(typeof(ExcuteType))]
public enum ExcuteType
{
    Once,Always
}

[Serializable]
public class RelicBase
{
    
    public float value;
    public float time;
    public ExcuteType excuteType;
    public List<RelicBase> extraRelic = new List<RelicBase>();

    private bool excuteFirst = false; //최초실행
    


    public  void Init(float _value,float _time,ExcuteType _excuteType)
    {
        excuteType = _excuteType;
        value = _value;
        time = _time;
    }
    

    public virtual void Excute(Character character)
    {
        if (excuteFirst && excuteType == ExcuteType.Once) return;
        excuteFirst = true;
        if (extraRelic.Count>0)
        {
            foreach (var _extraRelic in extraRelic)
            {
                Debug.Log($"extraRelic 실행 {_extraRelic.value}");
                _extraRelic.Excute(character);
            }
        }
    }

    public virtual void Excute()
    {
        if (excuteFirst && excuteType == ExcuteType.Once) return;
        excuteFirst = true;
        if (extraRelic.Count>0)
        {
            foreach (var _extraRelic in extraRelic)
            {
                _extraRelic.Excute();
            }
        }
    }

    public virtual IEnumerator ExcuteCor(Character character)
    {
        yield return null;
    }
    public virtual RelicBase Clone()
    {
        var clone = (RelicBase)Activator.CreateInstance(this.GetType());
        clone.value = this.value;
        clone.time = this.time;
        clone.excuteType = this.excuteType;
        clone.extraRelic = this.extraRelic.Select(r => r.Clone()).ToList(); // 깊은 복사
        return clone;
    }
    
    
}