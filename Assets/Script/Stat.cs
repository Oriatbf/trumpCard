using System;
using UnityEngine;

[Serializable]
public class Stat
{
    public CardType cardType;
    public CardRole cardRole;
    
    //유물 합산된 능력치가 들어가는 곳
    public int cardNum;
    public float hp;
    public float speed;
    public float damage;
    public float coolTime;
    public float criticalChance, criticalMultiplier;
    public float bulletSize;
    public float bulletSpeed; 
    public float attackCount, extraHitCount;  //attackCount는 전체 타격 횟수 extraHit은 한번 타격 횟수
}
