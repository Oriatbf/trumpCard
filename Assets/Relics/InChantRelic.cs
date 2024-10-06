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


        public enum Relic_Type { BloodSucking,fireDebuff,iceDebuff}

        public Relic_Type relic_Type;
        public int count;
        

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
       
        if(!excute.isExcute) SetInchant(so, relicType);
        SpecialAbilityType(so, relicType);
        Debug.Log("AAA");
    }

    public void SpecialAbilityType(CardStats so, RelicType relicType)
    {
        switch (relicType.relic_Type)
        {
            case Relic_Type.BloodSucking:
                if (so.bloodInchant.Count > 0)
                {
                    for (int i = 0; i < so.bloodInchant.Count; i++)
                    {
                        if (so.bloodInchant[i] == so.infor.cardNum)
                        {
                            Debug.Log("blooood" + so.bloodInchant[i]);
                            so.relicInfor.bloodSucking = true;
                            so.debuffs.Add(new FireDebuff { dotDamage = 2, duration = 5 });
                            break;
                        }
                    }

                }
                break;



        }
    }

    public void SetInchant(CardStats so, RelicType relicType)
    {
        if (!excute.isExcute)
        {
            switch (relicType.relic_Type)
            {
                case Relic_Type.BloodSucking:
                    CheckContains(so.bloodInchant,relicType);
                    break;
                case Relic_Type.fireDebuff:
                    CheckContains(so.fireInchant, relicType);
                    break;
                case Relic_Type.iceDebuff:
                    CheckContains(so.iceInchant, relicType);
                    break;

            }

            excute.isExcute = true;
            Debug.Log(excute.isExcute);
        } 
    }

    public void CheckContains(List<int> list,RelicType relicType)
    {
        for (int i = 0; i < relicType.count; i++)
        {
            if (list.Count > 12) return;
            int randomValue = Random.Range(0, 13);
            while (list.Contains(randomValue))
            {
                randomValue = Random.Range(0, 13);
            }

            list.Add(randomValue);
        }
    }
}
