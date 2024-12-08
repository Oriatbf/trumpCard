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
        public Sprite inchantIcon;
        public int count;

    }



    public RelicType[] relicType;

    public override void Active(CardStats so, RelicType relicType)
    {
       
      
        SpecialAbilityType(so, relicType);
        Debug.Log("AAA");
    }

    public void Inchant(CardStats so, RelicType relicType)
    {
        SetInchant(so, relicType);
    }

    public void SpecialAbilityType(CardStats so, RelicType relicType)
    {
        switch (relicType.relic_Type)
        {
            case Relic_Type.BloodSucking:
                if(HaveInchant(so.bloodInchant, so.infor.cardNum))
                {
                    Debug.Log("blooood" + so.infor.cardNum);
                    so.relicInfor.bloodSucking = true;
                }
                break;

            case Relic_Type.fireDebuff:
                if (HaveInchant(so.fireInchant, so.infor.cardNum))
                {
                    Debug.Log("fire" + so.infor.cardNum);
                    so.debuffs.Add(new FireDebuff { dotDamage = 2, duration = 5 });
                }
                break;

            case Relic_Type.iceDebuff:
                if (HaveInchant(so.iceInchant, so.infor.cardNum))
                {
                    Debug.Log("ice" + so.infor.cardNum);
                    so.debuffs.Add(new IceDebuff {duration = 0.5f});
                }
                break;
        }
    }

    public bool HaveInchant(List<int> inchantList,int cardNum)
    {
        if (inchantList.Count > 0)
        {
            for (int i = 0; i < inchantList.Count; i++)
            {
                if (inchantList[i] == cardNum)
                {
                    Debug.Log("inchant" + inchantList[i]);

                    return true;
                }
            }

        }
        return false;
    }

    public void SetInchant(CardStats so, RelicType relicType)
    {
       
        Debug.Log("익스큐트");
        List<int> cp_InchantList= new List<int>();
        switch (relicType.relic_Type)
        {
            case Relic_Type.BloodSucking:
                cp_InchantList = CheckContains(so.bloodInchant,relicType);
                break;
            case Relic_Type.fireDebuff:
                cp_InchantList = CheckContains(so.fireInchant, relicType);
                break;
            case Relic_Type.iceDebuff:
                cp_InchantList = CheckContains(so.iceInchant, relicType);
                break;

        }
       //UIManager.Inst.InchantCanvasAnim(cp_InchantList,relicType.inchantIcon);
         
    }

    public List<int> CheckContains(List<int> list,RelicType relicType)
    {
        List<int> cp_InchantList = new List<int>();
        for (int i = 0; i < relicType.count; i++)
        {
            if (list.Count > 12) return null;
            int randomValue = Random.Range(0, 13);
            while (list.Contains(randomValue))
            {
                randomValue = Random.Range(0, 13);
            }

            list.Add(randomValue);
            cp_InchantList.Add(randomValue);
        }
        return cp_InchantList;
    }
}
