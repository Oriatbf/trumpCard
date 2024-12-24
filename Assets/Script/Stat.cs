using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Stat
{
    public int cardNum;
    public CardType cardType;
    public CardRole cardRole;
    public Action statUpAction;
    
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
        
    }
    
    public StatsValue statValue = new StatsValue();

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


