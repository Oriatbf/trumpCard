using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BasicAbility.RelicType;

[CreateAssetMenu(fileName = "BasicRelic", menuName = "Scriptable SO/basicRelic", order = 1)]

public class BasicAbility : RelicSO
{
    [Serializable]
    public struct RelicType
    {

        public enum RelicKind { Add, RatioAdd}
        public enum Relic_Type { damage, speed, cool, P_Size, P_Count, criticalDam, criticalChance, hp, AutoHeal }
        public enum RoleAdd { Melee, Range, Both }

        public Relic_Type relic_Type;
        public RelicKind relicKind;
        public RoleAdd roleAdd;
        public float addValue, ratioValue;

    }

    public RelicType[] relicType;

    public void StartRelicActive(CardStats so, RelicType relicType)
    {
        AddType(so, relicType);    
    }

    public void StartRatioRelicActive(CardStats so, RelicType relicType)
    {
        RatioAddType(so, relicType);
    }

    public override void Active(CardStats so,RelicType relicType)
    {
        if(relicType.relicKind == RelicKind.Add)
             StartRelicActive(so, relicType);
        else
            StartRatioRelicActive(so,relicType);
    }


    public void AddType(CardStats so, RelicType relicType)
    {

        switch (relicType.relic_Type)
        {
            case Relic_Type.damage:
                if(ReturnRoleAbility(so,relicType))so.infor.damage += relicType.addValue; break;
            case Relic_Type.speed:
                if (ReturnRoleAbility(so, relicType)) so.infor.speed += relicType.addValue; break;
            case Relic_Type.cool:
                if (ReturnRoleAbility(so, relicType)) so.infor.coolTime += relicType.addValue; break;
            case Relic_Type.P_Count:
                so.infor.attackCount += (int)relicType.addValue; break;
            case Relic_Type.P_Size:
                so.relicInfor.size += relicType.addValue; break;
            case Relic_Type.criticalChance:
                so.relicInfor.criticalChance += (int)relicType.addValue; break;
            case Relic_Type.criticalDam:
                so.relicInfor.criticalDamage = relicType.addValue; break;
            case Relic_Type.hp:
                if (ReturnRoleAbility(so, relicType)) so.infor.hp += relicType.addValue; break;
            case Relic_Type.AutoHeal:
                so.relicInfor.characterHealth.autoHeal = true;
                so.relicInfor.characterHealth.autoHealSpeed = relicType.addValue;
                break;
        }
    }

    bool ReturnRoleAbility(CardStats so, RelicType relicType)
    {
        bool getAbility = false;
        switch (relicType.roleAdd)
        {
            case RoleAdd.Melee:
                getAbility = so.infor.role == CardStats.Role.Melee; break;
            case RoleAdd.Range:
                getAbility = so.infor.role == CardStats.Role.Range; break;
            case RoleAdd.Both:
                getAbility = true; break;
        }

        return getAbility;
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

    float RatioChangeValue(float abilValue, float ratioValue)
    {
        return abilValue * ratioValue * 0.01f;
    }
}
