using UnityEngine;

public class SpawnCreature : RelicBase
{
   public override void Excute(Character character)
   {
      Creature creature = Resources.Load<Creature>("Prefab/" + stringValue);
      Debug.Log(" 소환");
      character.creatureCustody.Init(creature);
      base.Excute(character);
   }
}
