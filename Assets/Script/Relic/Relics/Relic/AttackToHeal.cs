using UnityEngine;

public class AttackToHeal : RelicBase
{
    public override void Excute(Character character)
    {
        character.unitHealth.OnAttack +=()=>character.unitHealth.OnRecorvery(value);
        base.Excute(character);
    }
}
