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
    public string stringValue;
    public List<RelicBase> extraRelic = new List<RelicBase>();
    protected bool isActive = false;

    private bool excuteFirst = false; //최초실행
    


    public void Init(RelicData.Data data)
    {
        excuteType = data.excuteType;
        value = data.value;
        time = data.time;
        duration = data.duration;
        stringValue = data.stringValue;

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
                character.unitHealth.OnDamage += _action;
                break;
            case ExcuteType.OnHealed:
                character.unitHealth.OnHeal += _action;
                break;
            case ExcuteType.OnAttack:
                character.unitHealth.OnAttack += _action;
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
        clone.stringValue = this.stringValue;
        clone.extraRelic = this.extraRelic.Select(r => r.Clone()).ToList(); // 깊은 복사
        return clone;
    }
    
    
}