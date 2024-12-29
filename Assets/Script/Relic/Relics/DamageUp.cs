using UnityEngine;

public class DamageUp : RelicBase
{
    public override void Excute(Character character)
    {
        character.stat.statUpAction += () => character.stat.originStatValue.damage += value;
        base.Excute(character);
    }
}
