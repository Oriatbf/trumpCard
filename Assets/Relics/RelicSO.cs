using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

[CreateAssetMenu(fileName = "RelicSO", menuName = "Scriptable SO/Relic", order = 1)]

public class RelicSO : ScriptableObject
{
    public string relicName;
    [Serializable]
    public struct RelicType
    {
        public enum ActiveType { Start, Attack, Reload,Moving,Update };      
        public enum SequenceType { Immediately,Later };
        public enum AddtionType { DamageType, CoolTimeType,SpeedType,HpType,ProjectileType};


        public enum DamageRelic { Damage, RatioDamage, DamInPropotionToSpeed , LoseHpUpDam };
        public enum CoolTimeRelic { CoolTime };
        public enum HpRelic { AutoHeal , MaxHp };
        public enum SpeedRelic { Speed, LoseHpUpSpeed };
        public enum ProjectileRelic { projectileSize, projectileAdd };

        public ActiveType activeType;
        [ShowIfEnum("activeType", (int)ActiveType.Start)]
        public SequenceType sequenceType;
        public AddtionType addtionType;
        

        [ShowIfEnum("addtionType", (int)AddtionType.DamageType)]
        public DamageRelic damageRelic;
        [ShowIfEnum("addtionType", (int)AddtionType.CoolTimeType)] 
        public CoolTimeRelic coolTimeRelic;
        [ShowIfEnum("addtionType", (int)AddtionType.HpType)] 
        public HpRelic hpRelic;
        [ShowIfEnum("addtionType", (int)AddtionType.SpeedType)] 
        public SpeedRelic speedRelic;
        [ShowIfEnum("addtionType", (int)AddtionType.ProjectileType)] 
        public ProjectileRelic projectileRelic;


        [ShowIfEnum("addtionType", (int)AddtionType.DamageType)]
        public float addtionDamage;
        [ShowIfEnum("addtionType", (int)AddtionType.DamageType)]
        public float addtionRatio;
        [ShowIfEnum("addtionType", (int)AddtionType.DamageType)]
        public float addtionDamageRatio;
        [ShowIfEnum("addtionType", (int)AddtionType.DamageType)]
        public float loseHp, loseHpPerDam;


        [ShowIfEnum("addtionType", (int)AddtionType.CoolTimeType)]
        public float addtionCoolTime;

        [ShowIfEnum("addtionType", (int)AddtionType.HpType)]
        public float addtionMaxHp;
        [ShowIfEnum("addtionType", (int)AddtionType.HpType)]
        public float perValue;

        [ShowIfEnum("addtionType", (int)AddtionType.SpeedType)]
        public float addtionSpeed;
        [ShowIfEnum("addtionType", (int)AddtionType.SpeedType)]
        public float loseHp2, loseHpPerSpeed;

        [ShowIfEnum("addtionType", (int)AddtionType.ProjectileType)]
        public float pSize;
        [ShowIfEnum("addtionType", (int)AddtionType.ProjectileType)]
        public int pCount;


        public GameObject floor;
    }

    public RelicType[] relicType;


    public void StartRelicActive(CardStats so,RelicType relicType)
    {
        Debug.Log(so);
        
        Increase(so, relicType);   
    }

    public void StartRatioRelicActive(CardStats so, RelicType relicType)
    {
        Debug.Log(so);

        RatioIncrease(so, relicType);
    }



    public void Increase(CardStats so,RelicType relicType)
    {
        switch (relicType.damageRelic)
        {
            case RelicType.DamageRelic.Damage:
                so.infor.damage += relicType.addtionDamage;
                break;
        }

        switch (relicType.coolTimeRelic)
        {
            case RelicType.CoolTimeRelic.CoolTime:
                so.infor.coolTime += relicType.addtionCoolTime;
                break;
        }

        switch (relicType.hpRelic)
        {
            case RelicType.HpRelic.MaxHp:
                so.infor.hp += relicType.addtionMaxHp;
                break;
            case RelicType.HpRelic.AutoHeal:
                so.relicInfor.characterHealth.autoHeal = true;
                so.relicInfor.characterHealth.autoHealSpeed = relicType.perValue;
                break;
        }

        switch (relicType.speedRelic)
        {
            case RelicType.SpeedRelic.Speed:
                so.infor.speed += relicType.addtionSpeed;
                break;
        }

        switch (relicType.projectileRelic)
        {
            case RelicType.ProjectileRelic.projectileSize:
                so.relicInfor.size = relicType.pSize;
                break;
            case RelicType.ProjectileRelic.projectileAdd:
                so.infor.attackCount += relicType.pCount;
                break;
        }

    }


    public void UpdateRelic(CardStats so, RelicType relicType)
    {
        switch (relicType.damageRelic)
        {
            case RelicType.DamageRelic.LoseHpUpDam:
                so.infor.damage +=  Mathf.Floor((so.relicInfor.characterHealth.maxHp-so.relicInfor.characterHealth.curHp)/relicType.loseHp) * relicType.loseHpPerDam;
                break;        
        }

        switch (relicType.speedRelic)
        {
            case RelicType.SpeedRelic.LoseHpUpSpeed:
                so.infor.speed += Mathf.Floor((so.relicInfor.characterHealth.maxHp - so.relicInfor.characterHealth.curHp) / relicType.loseHp2) * relicType.loseHpPerSpeed;
                break;
        }

    }

    public void RatioIncrease(CardStats so, RelicType relicType)
    {
        switch (relicType.damageRelic)
        {
            case RelicType.DamageRelic.DamInPropotionToSpeed:
                so.infor.damage += so.infor.speed * relicType.addtionRatio * 0.01f;
                break;
            case RelicType.DamageRelic.RatioDamage:
                so.infor.damage += so.infor.damage * relicType.addtionDamageRatio * 0.01f;
                break;
        }
   
    }

    public void Flooring(Transform trans,RelicType relicType)
    {
        GameObject a= Instantiate(relicType.floor, trans.position, Quaternion.identity);
        Destroy(a, 0.5f);
    }

}
