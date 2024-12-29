using UnityEngine;

public class BulletSizeUp : RelicBase
{
    public override void Excute(Character character)
    {
        character.stat.statUpAction += ()=>character.stat.originStatValue.bulletSize += value;
        base.Excute(character);
    }
}
