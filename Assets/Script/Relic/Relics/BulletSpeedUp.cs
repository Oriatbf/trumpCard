using UnityEngine;

public class BulletSpeedUp : RelicBase
{
    public override void Excute(Character character)
    {
        character.stat.statUpAction += ()=>character.stat.originStatValue.bulletSpeed += value;
        base.Excute(character);
    }
}
