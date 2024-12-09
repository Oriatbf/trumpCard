using UnityEngine;

public class HpUp : RelicBase
{
    public override void Excute(Character character)
    {
        character.stat.statUpAction += () => character.stat.statValue.hp += value;
        base.Excute(character);
    }
}
