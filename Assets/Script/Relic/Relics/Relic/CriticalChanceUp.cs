using UnityEngine;

public class CriticalChanceUp : RelicBase
{
    public override void Excute(Character character)
    {
        character.stat.statUpAction += ()=>character.stat.originStatValue.criticalChance += value;
        base.Excute(character);
    }
}
