using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class HealPerTime : RelicBase
{
    public override void Excute(Character character)
    {
        base.Excute(character);
    }

    public override IEnumerator ExcuteCor(Character character)
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            character.unitHealth.OnRecorvery(value);
        }
    }

 
}
