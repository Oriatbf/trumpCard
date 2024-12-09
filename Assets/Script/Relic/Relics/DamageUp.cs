using UnityEngine;

public class DamageUp : RelicBase
{
    public override void Excute(Character character)
    {
        character.stat.statUpAction += () => character.stat.statValue.damage += value;
        base.Excute(character);
    }
}
