using UnityEngine;

public class CreatureAbility : RelicBase
{
    public override void Excute(Character character)
    {
      
        character.creatureCustody.Init(value);
        base.Excute(character);
    }
}
