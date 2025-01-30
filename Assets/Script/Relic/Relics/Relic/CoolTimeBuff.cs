using DG.Tweening;
using UnityEngine;

public class CoolTimeBuff : RelicBase
{
    
    public override void Excute(Character character)
    {
        SetAction(character,()=>CoolTimeBuffActive(character));
        base.Excute(character);
    }



    private void CoolTimeBuffActive(Character character)
    {
        if (isActive) return;
        isActive = true;
        character.stat.buffDebuffValue.coolTime += value;
        DOVirtual.DelayedCall(duration, () =>
        {
            character.stat.buffDebuffValue.coolTime -= value;
            DOVirtual.DelayedCall(time, () => isActive = false);
        });
       
    }

}