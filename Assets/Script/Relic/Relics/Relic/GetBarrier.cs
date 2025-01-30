using Unity.Mathematics;
using UnityEngine;

public class GetBarrier : RelicBase
{
    private Health barrier;
    public override void Excute(Character character)
    {
        barrier = Resources.Load<Health>("Prefab/Barrier");
        SetAction(character,()=>Action(character));
        
        base.Excute(character);
    }

    private void Action(Character character)
    {
        Health b =  Object.Instantiate(barrier,character.transform.position,quaternion.identity);
        b.ResetHp(value,value);
        b.characterType = character.unitHealth.characterType;
        b.transform.parent = character.transform;
    }
}
