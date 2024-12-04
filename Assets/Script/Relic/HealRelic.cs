using UnityEngine;

public class HealRelic : RelicBase
{
    public override void Excute(Character character)
    {
        
        character.health.OnDamage +=()=>character.health.OnRecorvery(value);
        
    }
}
