using System.Collections;
using UnityEngine;

public class MuryangGongcheo : RelicBase
{
    private GameObject effect;
    public override IEnumerator ExcuteCor(Character character)
    {
        effect = Resources.Load<GameObject>("Effect/MuryangGongcheoEffect");
        
        Character opponent =  GameManager.Inst.GetOpponent(character);
        while (!character.unitHealth.isInv)
        {
            yield return new WaitForSeconds(time);
            
            opponent.unitHealth.GetDamage(opponent.unitHealth.curHp/2);
            Object.Instantiate(effect);
        }
    }
}
