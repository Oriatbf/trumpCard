using UnityEngine;

public class CoolTimeUp : RelicBase
{
    public override void Excute(Character character)
    {
        character.stat.statUpAction += ()=>character.stat.statValue.coolTime += value;
        base.Excute(character);
    }
}
