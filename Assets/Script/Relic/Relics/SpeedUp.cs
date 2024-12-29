using UnityEngine;

public class SpeedUp : RelicBase
{
    public override void Excute(Character character)
    {
        character.stat.statUpAction += ()=>character.stat.originStatValue.speed += value;
        base.Excute(character);
    }
}
