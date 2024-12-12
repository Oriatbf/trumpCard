using DG.Tweening;
using UnityEngine;

public class Sandevistan : RelicBase
{
    private float coolTime = 10f;
    private bool active;
    public override void Excute(Character character)
    {
        character.health.OnDamage+=()=> TimeChange(character);
       
        
        base.Excute(character);
    }

    private void TimeChange(Character character)
    {
        if (active) return;
        active = true;
        TimeManager.ChangeTimeSpeed(0.3f);
        VolumeManager.Inst.SandevistanEffect(time);
        var defaultSpeed = character.stat.statValue.speed;
        character.stat.statValue.speed = defaultSpeed/Time.timeScale;
        character.dashEffect.ActiveDashEffect(time);
        DOVirtual.DelayedCall(time, () =>
        {
            TimeManager.ChangeTimeSpeed(1f);
            character.stat.statValue.speed = defaultSpeed;
            DOVirtual.DelayedCall(coolTime, () => active = false);
        });
    }
    
    public override RelicBase Clone()
    {
        var clone = (Sandevistan)base.Clone();
        clone.coolTime = this.coolTime;
        clone.active = this.active;
        return clone;
    }
    
}
