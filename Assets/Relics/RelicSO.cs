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

        public enum RelicKind {Add,RatioAdd,Inchant ,Melee,Slime}
        public enum Relic_Type {damage,speed,cool,P_Size,P_Count,criticalDam,criticalChance,hp,AutoHeal}
        public enum RoleAdd {Melee,Range,Both }

        public Relic_Type relic_Type;
        public RelicKind addType;
        public RoleAdd roleAdd;
        public float addValue, ratioValue;

    }

    public RelicType[] relicType;


    public void StartRelicActive(CardStats so,RelicType relicType)
    {     
        switch (relicType.addType)
        {
            case RelicType.RelicKind.Add:
                AddType(so,relicType); break;
            case RelicType.RelicKind.Melee:
                MeleeType(so, relicType); break;
        }
    }

    public void StartRatioRelicActive(CardStats so, RelicType relicType)
    {
        RatioAddType(so, relicType);
    }


    public void AddType(CardStats so, RelicType relicType)
    {

        switch (relicType.relic_Type)
        {
            case Relic_Type.damage:
                so.infor.damage += relicType.addValue; break;
            case Relic_Type.speed:
                so.infor.speed += relicType.addValue; break;
            case Relic_Type.cool:
                so.infor.coolTime += relicType.addValue; break;
            case Relic_Type.P_Count:
                so.infor.attackCount += (int)relicType.addValue; break;
            case Relic_Type.P_Size:
                so.relicInfor.size = relicType.addValue; break;
            case Relic_Type.criticalChance:
                so.relicInfor.criticalChance += (int)relicType.addValue; break;
            case Relic_Type.criticalDam:
                so.relicInfor.criticalDamage += relicType.addValue; break;
            case Relic_Type.hp:
                so.infor.hp += relicType.addValue; break;
            case Relic_Type.AutoHeal:
                so.relicInfor.characterHealth.autoHeal = true;
                so.relicInfor.characterHealth.autoHealSpeed = relicType.addValue;
                break;
        }
    }

  
    public void MeleeType(CardStats so, RelicType relicType)
    {
        if(so.infor.attackType == CardStats.AttackType.Melee)
        {
            switch (relicType.relic_Type)
            {
                case Relic_Type.damage:
                    so.relicInfor.meleeDam += relicType.addValue; break;
                case Relic_Type.speed:
                    so.relicInfor.meleeSpeed += relicType.addValue; break;
                case Relic_Type.cool:
                    so.relicInfor.meleeCool += relicType.addValue; break;
            }
        }
      
    }

    public void RatioAddType(CardStats so, RelicType relicType)
    {
        switch (relicType.relic_Type)
        {
            case Relic_Type.damage:
                so.infor.damage += RatioChangeValue(so.infor.damage, relicType.ratioValue); break;
            case Relic_Type.speed:
                so.infor.speed += RatioChangeValue(so.infor.speed, relicType.ratioValue); break;
            case Relic_Type.cool:
                so.infor.coolTime += RatioChangeValue(so.infor.coolTime, relicType.ratioValue); break;
        }
    }

    float RatioChangeValue(float abilValue ,float ratioValue)
    {
        return abilValue * ratioValue*0.01f;
    }


    

}
