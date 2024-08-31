using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SpecialAbility.RelicType;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "SpecialRelic", menuName = "Scriptable SO/specialRelic", order = 1)]
public class SpecialAbility : RelicSO
{
    [Serializable]
    public struct RelicType
    {


        public enum Relic_Type { flooringBullet, SpawnSlime,UpGradeA }

        public Relic_Type relic_Type;

        public float addValue;
        public GameObject slime;

    }

    public RelicType[] relicType;

    public override void Active(CardStats so, RelicType relicType)
    {
        SpecialAbilityType(so, relicType);
    }

    public void SpecialAbilityType(CardStats so, RelicType relicType)
    {
        switch (relicType.relic_Type)
        {
            case Relic_Type.flooringBullet:
                so.relicInfor.isFlooring = true; so.relicInfor.floorTickDamage = relicType.addValue; break;
            case Relic_Type.SpawnSlime:
                Instantiate(relicType.slime, (so.relicInfor.characterTrans.position + (Vector3)(Random.insideUnitCircle)).normalized * 2f, Quaternion.identity);break;
            case Relic_Type.UpGradeA:
                if(so.infor.cardNum== 0)
                {
                    so.infor.damage += 5;
                    so.infor.hp += 20;
                    so.relicInfor.relicPlusHealth += 20;
                    so.infor.speed += 2;
                    Debug.Log("AAA");
                }
                break;

        }
    }
}
