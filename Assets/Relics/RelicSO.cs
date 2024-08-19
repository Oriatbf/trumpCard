using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using static RelicSO.RelicType;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "RelicSO", menuName = "Scriptable SO/Relic", order = 1)]

public class RelicSO : ScriptableObject
{
    public string relicName;
    public Sprite relicIcon;
    public string relicInfor;
    public enum Rarity {Common,Rare,Epic };
    public Rarity rarity;
    [Serializable]
    public struct RelicType
    {
        
        public enum ActiveType { Start, Attack, Reload,Moving,Update };      
        public enum SequenceType { Immediately,Later };
        public enum AddtionType 
        {
            Add_Dam,Add_Per_Dam,loseHp_Per_Dam,Flooring,Speed_Per_Dam,
            Add_Cool,
            Add_MaxHp,AutoHeal,
            Add_Speed,loseHp_Per_Speed,
            P_Size,P_Count,
            CriticalDamage, Critical_Value,
            Slime,
            Melee_Add_Dam,Melee_Add_Cool,Melee_Add_MaxHp
        };

        public ActiveType activeType;
        [ShowIfEnum("activeType", (int)ActiveType.Start)]
        public SequenceType sequenceType;
        public AddtionType addtionType;
        


        [ShowIfEnum("addtionType", (int)AddtionType.Add_Dam)]
        public float add_Dam;
        [ShowIfEnum("addtionType", (int)AddtionType.Add_Per_Dam)]
        public float add_Per_Dam;
        [ShowIfEnum("addtionType", (int)AddtionType.loseHp_Per_Dam)]
        public float L_P_D_Hp, loseHp_Per_Dam;
        [ShowIfEnum("addtionType", (int)AddtionType.Flooring)]
        public float floorTickDamage;
        [ShowIfEnum("addtionType", (int)AddtionType.Flooring)]
        public bool isFloor;
        [ShowIfEnum("addtionType", (int)AddtionType.Speed_Per_Dam)]
        public float speed_Per_Dam;
        [ShowIfEnum("addtionType", (int)AddtionType.Critical_Value)]
        public int critical_Value;
        [ShowIfEnum("addtionType", (int)AddtionType.CriticalDamage)]
        public float criticalDamage;


        [ShowIfEnum("addtionType", (int)AddtionType.Add_Cool)]
        public float add_Cool;

        [ShowIfEnum("addtionType", (int)AddtionType.Add_MaxHp)]
        public float add_MaxHp;
        [ShowIfEnum("addtionType", (int)AddtionType.AutoHeal)]
        public bool isAutoHeal;
        [ShowIfEnum("addtionType", (int)AddtionType.AutoHeal)]
        public float autoHeal_perValue;

        [ShowIfEnum("addtionType", (int)AddtionType.Add_Speed)]
        public float add_Speed;
        [ShowIfEnum("addtionType", (int)AddtionType.loseHp_Per_Speed)]
        public float L_P_S_Hp, loseHp_Per_Speed;

        [ShowIfEnum("addtionType", (int)AddtionType.P_Size)]
        public float p_Size;
        [ShowIfEnum("addtionType", (int)AddtionType.P_Count)]
        public int p_Count;

        [ShowIfEnum("addtionType", (int)AddtionType.Slime)]
        public GameObject slime;

        [ShowIfEnum("addtionType", (int)AddtionType.Melee_Add_Dam)]
        public float melee_Add_Dam;
        [ShowIfEnum("addtionType", (int)AddtionType.Melee_Add_Cool)]
        public float melee_Add_Cool;
        [ShowIfEnum("addtionType", (int)AddtionType.Melee_Add_MaxHp)]
        public float melee_Add_MaxHp;


    }

    public RelicType[] relicType;


    public void StartRelicActive(CardStats so,RelicType relicType)
    {
        Debug.Log(so);
        
        StartRelic(so, relicType);   
    }

    public void StartRatioRelicActive(CardStats so, RelicType relicType)
    {
        Debug.Log(so);

        RatioIncrease(so, relicType);
    }



    public void StartRelic(CardStats so,RelicType relicType)
    {
        CardStats.AttackType attackType = so.infor.attackType;

        switch (relicType.addtionType)
        {        
            case AddtionType.Add_Dam:
                so.infor.damage += relicType.add_Dam;
                break;
            case AddtionType.Flooring:
                so.relicInfor.isFlooring = relicType.isFloor;
                so.relicInfor.floorTickDamage = relicType.floorTickDamage;
                break;
            case AddtionType.Add_Cool:
                so.infor.coolTime += relicType.add_Cool;
                break;
            case AddtionType.Add_MaxHp:
                so.infor.hp += relicType.add_MaxHp;
                break;
            case AddtionType.AutoHeal:
                so.relicInfor.characterHealth.autoHeal = relicType.isAutoHeal;
                so.relicInfor.characterHealth.autoHealSpeed = relicType.autoHeal_perValue;
                break;
            case AddtionType.Add_Speed:
                so.infor.speed += relicType.add_Speed;
                break;
            case AddtionType.P_Size:
                so.relicInfor.size = relicType.p_Size;
                break;
            case AddtionType.P_Count:
                so.infor.attackCount += relicType.p_Count;
                break;
            case AddtionType.Critical_Value:
                so.relicInfor.criticalChance += relicType.critical_Value;
                break;
            case AddtionType.CriticalDamage:
                so.relicInfor.criticalDamage = relicType.criticalDamage;
                break;
            case AddtionType.Slime:
                Instantiate(relicType.slime,(so.relicInfor.characterTrans.position + (Vector3)(Random.insideUnitCircle)).normalized * 2f,Quaternion.identity);
                break;
            case AddtionType.Melee_Add_Dam:
                if (attackType == CardStats.AttackType.Melee && attackType == CardStats.AttackType.MeleeSting)
                    so.infor.damage += relicType.melee_Add_Dam;
                break;
            case AddtionType.Melee_Add_Cool:
                if (attackType == CardStats.AttackType.Melee && attackType == CardStats.AttackType.MeleeSting)
                    so.infor.coolTime += relicType.melee_Add_Cool;
                break;
            case AddtionType.Melee_Add_MaxHp:
                if (attackType == CardStats.AttackType.Melee && attackType == CardStats.AttackType.MeleeSting)
                    so.infor.hp += relicType.melee_Add_MaxHp;
                break;
        }

    }


    public void UpdateRelic(CardStats so, RelicType relicType)
    {
        switch (relicType.addtionType)
        {
            case AddtionType.loseHp_Per_Dam:
                so.infor.damage +=  Mathf.Floor((so.relicInfor.characterHealth.maxHp-so.relicInfor.characterHealth.curHp)/relicType.L_P_D_Hp) * relicType.loseHp_Per_Dam;
                break;
            case AddtionType.loseHp_Per_Speed:
                so.infor.speed += Mathf.Floor((so.relicInfor.characterHealth.maxHp - so.relicInfor.characterHealth.curHp) / relicType.L_P_S_Hp) * relicType.loseHp_Per_Speed;
                break;
        }

     

    }

    public void RatioIncrease(CardStats so, RelicType relicType)
    {
        switch (relicType.addtionType)
        {
            case AddtionType.Speed_Per_Dam:
                so.infor.damage += so.infor.speed * relicType.speed_Per_Dam * 0.01f;
                break;
            case AddtionType.Add_Per_Dam:
                so.infor.damage += so.infor.damage * relicType.add_Per_Dam * 0.01f;
                break;
        }
   
    }

   

}
