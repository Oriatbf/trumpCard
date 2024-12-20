using System.Collections;
using UnityEngine;

public class BlackFlash : RelicBase
{
    private GameObject effect;
    public override void Excute(Character character)
    {
        base.Excute(character);
    }

    public override IEnumerator ExcuteCor(Character character)
    {
        effect = Resources.Load<GameObject>("Effect/BlackFlash");
        Character opponent =  GameManager.Inst.GetOpponent(character);
        while (!character.health.isInv)
        {
            yield return new WaitForSeconds(time);
            Vector3 dir = opponent.transform.position - character.transform.position;
            Object.Instantiate(effect, opponent.transform);
            character.transform.position = opponent.transform.position + (dir.normalized);
            opponent.health.GetDamage(value);
        }
    }
}
