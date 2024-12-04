using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Stat
{
    public int cardNum;
    public CardType cardType;
    public CardRole cardRole;
    
    [Serializable]
    public class StatsValue
    {
       
        public float hp;
        public float speed;
        public float damage;
        public float coolTime;
        public float criticalChance, criticalMultiplier;
        public float bulletSize;
        public float bulletSpeed; 
        public float attackCount, extraHitCount;  
    }

    public StatsValue basicStatValue = new StatsValue();
    public StatsValue relicStatValue = new StatsValue();




}
