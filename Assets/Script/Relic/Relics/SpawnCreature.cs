using UnityEngine;

public class SpawnCreature : RelicBase
{
   public override void Excute(Character character)
   {
      Creature creature = Resources.Load<Creature>("Prefab/Slime_Creature");
      
      character.creatureCustody.Init(creature);
      base.Excute(character);
   }
}
