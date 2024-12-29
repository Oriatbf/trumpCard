using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GoogleSheet.Core.Type;
using UnityEngine;

[UGS(typeof(ExcuteType))]
public enum ExcuteType
{
    OnGet,OnGamble,OnDamaged,OnAttack,OnHealed,NoCondition
}

[Serializable]
public class RelicBase
{
    
    public float value;
    public float time;
    public float duration;
    public ExcuteType excuteType;
    public List<RelicBase> extraRelic = new List<RelicBase>();

    private bool excuteFirst = false; //최초실행
    


    public void Init(float _value,float _time,float _duration,ExcuteType _excuteType)
    {
        excuteType = _excuteType;
        value = _value;
        time = _time;
        duration = _duration;

    }
    

    public virtual void Excute(Character character)
    {
        Debug.Log(excuteType);
        if (excuteFirst && excuteType == ExcuteType.OnGet) return;
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
        if (excuteFirst && excuteType == ExcuteType.OnGet) return;
        excuteFirst = true;
        if (extraRelic.Count>0)
        {
            foreach (var _extraRelic in extraRelic)
            {
                _extraRelic.Excute();
            }
        }
    }

    public void SetAction(Character character,Action _action) //게임 시작 시 실행할때만 해당
    {
        switch (excuteType)
        {
            case ExcuteType.OnGet:
                break;
            case ExcuteType.NoCondition:
                break;
            case ExcuteType.OnGamble:
                character.stat.statUpAction += _action;
                break;
            case ExcuteType.OnDamaged:
                character.health.OnDamage += _action;
                break;
            case ExcuteType.OnHealed:
                character.health.OnHeal += _action;
                break;
            case ExcuteType.OnAttack:
                character.health.OnAttack += _action;
                break;

                
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
        clone.duration = this.duration;
        clone.excuteType = this.excuteType;
        clone.extraRelic = this.extraRelic.Select(r => r.Clone()).ToList(); // 깊은 복사
        return clone;
    }
    
    
}