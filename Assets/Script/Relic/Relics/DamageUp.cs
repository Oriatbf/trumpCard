using UnityEngine;

public class DamageUp : RelicBase
{
    public override void Excute(Character character)
    {
        character.stat.basicStatValue.damage += value;
        base.Excute(character);
    }
}
