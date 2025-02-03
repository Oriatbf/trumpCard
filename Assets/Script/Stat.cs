using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Stat
{
    public int cardNum;
    public int bulletIndex;
    public string cardName;
    public CardType cardType;
    public CardRole cardRole;
    public Action statUpAction; 
    public StatsValue originStatValue = new StatsValue();
    public StatsValue buffDebuffValue = new StatsValue();
    
    [Serializable]
    public struct StatsValue
    {

        public float hp;
        public float speed;
        public float damage;
        public float coolTime;
        public float criticalChance, criticalMultiplier;
        public float bulletSize;
        public float bulletSpeed; 
        public float attackCount, extraHitCount;
        
        public static StatsValue operator +(StatsValue a, StatsValue b)
        {
            return new StatsValue
            {
                hp = a.hp + b.hp,
                speed = a.speed + b.speed,
                damage = a.damage + b.damage,
                coolTime = a.coolTime + b.coolTime,
                criticalChance = a.criticalChance + b.criticalChance,
                criticalMultiplier = a.criticalMultiplier + b.criticalMultiplier,
                bulletSize = a.bulletSize + b.bulletSize,
                bulletSpeed = a.bulletSpeed + b.bulletSpeed,
                attackCount = a.attackCount + b.attackCount,
                extraHitCount = a.extraHitCount + b.extraHitCount
            };
        }
        
        public static StatsValue operator *(StatsValue a, float b)
        {
            return new StatsValue
            {
                hp = a.hp *b,
                speed = a.speed * b,
                damage = a.damage * b,
                coolTime = a.coolTime * b,
                criticalChance = a.criticalChance * b,
                criticalMultiplier = a.criticalMultiplier * b,
                bulletSize = a.bulletSize * b,
                bulletSpeed = a.bulletSpeed * b,
                attackCount = a.attackCount * b,
                extraHitCount = a.extraHitCount * b
            };
        }
    }

    public StatsValue FinalValue()
    {
        StatsValue stat = originStatValue + buffDebuffValue;
        return stat;
    }
    

    public void Action()
    {
        if (statUpAction == null)
        {
            Debug.Log("No Action");
        }
        else
        {
            statUpAction?.Invoke();
            Debug.Log("Have Action");
        }
       
    }

}


