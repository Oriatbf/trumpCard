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
        var defaultSpeed = character.stat.basicStatValue.speed;
        character.stat.basicStatValue.speed = defaultSpeed/Time.timeScale;
        DOVirtual.DelayedCall(value, () =>
        {
            TimeManager.ChangeTimeSpeed(1f);
            character.stat.basicStatValue.speed = defaultSpeed;
            DOVirtual.DelayedCall(coolTime, () => active = false);
        });
    }
}
