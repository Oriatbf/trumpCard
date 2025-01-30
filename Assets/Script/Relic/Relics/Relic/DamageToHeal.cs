using Unity.VisualScripting;
using UnityEngine;
using VInspector.Libs;

public class DamageToHeal : RelicBase
{
    
    public override void Excute(Character character)
    {
        character.unitHealth.OnDamage +=()=>character.unitHealth.OnRecorvery(value);
        base.Excute(character);
    }
}
