using DG.Tweening;
using UnityEngine;

public class Sandevistan : RelicBase
{
    private bool active;
    public override void Excute(Character character)
    {
        character.unitHealth.OnDamage+=()=> TimeChange(character);
       
        
        base.Excute(character);
    }

    private void TimeChange(Character character)
    {
        if (active) return;
        active = true;
        TimeManager.Inst.ChangeTimeSpeedCor(0.3f,duration);
        VolumeManager.Inst.SandevistanEffect(duration);
        var defaultSpeed = character.stat.originStatValue.speed;
        character.stat.originStatValue.speed = defaultSpeed/Time.timeScale;
        character.dashEffect.ActiveDashEffect(duration);
        DOVirtual.DelayedCall(duration, () =>
        {
            character.stat.originStatValue.speed = defaultSpeed;
            DOVirtual.DelayedCall(time, () => active = false);
        });
    }
    
    public override RelicBase Clone()
    {
        var clone = (Sandevistan)base.Clone();
        clone.active = this.active;
        return clone;
    }
    
}
