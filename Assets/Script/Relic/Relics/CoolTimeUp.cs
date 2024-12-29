using UnityEngine;

public class CoolTimeUp : RelicBase
{
    public override void Excute(Character character)
    {
        character.stat.statUpAction += ()=>character.stat.originStatValue.coolTime += value;
        base.Excute(character);
    }
}
