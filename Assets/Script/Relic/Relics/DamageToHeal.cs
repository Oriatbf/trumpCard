using Unity.VisualScripting;
using UnityEngine;
using VInspector.Libs;

public class DamageToHeal : RelicBase
{
    
    public override void Excute(Character character)
    {
        character.health.OnDamage +=()=>character.health.OnRecorvery(value);
        base.Excute(character);
    }
}
