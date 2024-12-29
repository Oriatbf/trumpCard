using UnityEngine;

public class HpUp : RelicBase
{
    public override void Excute(Character character)
    {
        Debug.Log(character.stat);
        character.stat.statUpAction += () =>  Debug.Log($"HpUp 실행 {value}");
        character.stat.statUpAction += () => character.stat.originStatValue.hp += value;
        base.Excute(character);
    }
}
