using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InChantRelic.RelicType;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "InchantRelic", menuName = "Scriptable SO/inchantRelic", order = 1)]

public class InChantRelic : RelicSO
{
    [Serializable]
    public struct RelicType
    {


        public enum Relic_Type { BloodSucking }

        public Relic_Type relic_Type;

        

    }

    [Serializable]
    public class Excute
    {
        public bool isExcute;
    }



    public Excute excute;
    public RelicType[] relicType;

    public override void Active(CardStats so, RelicType relicType)
    {
        Debug.Log("AAA");
        if(!excute.isExcute) SetInchant(so, relicType);
        SpecialAbilityType(so, relicType);
    }

    public void SpecialAbilityType(CardStats so, RelicType relicType)
    {
        switch (relicType.relic_Type)
        {
            case Relic_Type.BloodSucking:
                for(int i = 0; i<RelicManager.Inst.playerBloodInchant.Count; i++)
                {
                   
                    if (RelicManager.Inst.playerBloodInchant[i] == so.infor.cardNum)
                    {
                        Debug.Log("blooood");
                        so.relicInfor.bloodSucking = true;
                        break;
                    }
                   
                } break;

        }
    }

    public void SetInchant(CardStats so, RelicType relicType)
    {
        if (!excute.isExcute)
        {
            switch (relicType.relic_Type)
            {
                case Relic_Type.BloodSucking:
                    RelicManager.Inst.playerBloodInchant.Add(Random.Range(0, 13)); break;

            }

            excute.isExcute = true;
            Debug.Log(excute.isExcute);
        }
       

        
    }
}
