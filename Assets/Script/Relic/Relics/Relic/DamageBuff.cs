using System.Collections;
using DG.Tweening;
using UnityEngine;

public class DamageBuff : RelicBase
{
    private bool active = false;

    public override void Excute(Character character)
    {
        SetAction(character,()=>DamageBuffActive(character));
        base.Excute(character);
    }



    private void DamageBuffActive(Character character)
    {
        if (active) return;
        active = true;
        character.stat.buffDebuffValue.damage += value;
        DOVirtual.DelayedCall(duration, () =>
        {
            character.stat.buffDebuffValue.damage -= value;
            DOVirtual.DelayedCall(time, () => active = false);
        });
       
    }
    
    public override RelicBase Clone()
    {
        var clone = (DamageBuff)base.Clone();
        clone.active = this.active;
        return clone;
    }
}
