using UnityEngine;

public class Debuff : RelicBase
{
    public override void Excute(Character character)
    {
        Debug.Log("디버프 추가중");
        character.debuffs.Add(new DotDamage(duration,value));
        base.Excute();
        
    }

}
