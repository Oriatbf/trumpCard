using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting;
using UnityEngine;
using VInspector.Libs;

public class HealRelic : RelicBase
{
    
    public override void Excute(Character character)
    {
        character.health.OnDamage += () => Attack.Inst.Shoot(character.shootInfor);
        //character.health.OnDamage +=()=>character.health.OnRecorvery(value);

    }
}
