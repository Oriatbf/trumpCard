using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public static CharacterStats Inst;


    public struct Stats
    {
        public float speed, hp, coolTime, damage;
        public float addtionSpeed,addtionHp,addtionCoolTime,addtionDamage;
        public float finalSpeed,finalHp,finalCoolTime,finalDamage;

        public void setStats(float speed,float hp,float coolTime, float damage)
        {
            this.speed = speed;
            this.hp = hp;
            this.coolTime = coolTime;
            this.damage = damage;
        }

        public void addtionAbility()
        {
            finalSpeed = speed + addtionSpeed;
            finalHp = hp + addtionHp;
            finalCoolTime= coolTime + addtionCoolTime;
            finalDamage = damage+ addtionDamage;
        }
    }
    public GameObject player,enemy,curCharacter;
    public Stats playerStat,enemyStat;
    public float damage, coolTime,speed,hp;

    private void Awake()
    {
        Inst= this;
        
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    public void StatsApply(float speed,float hp,float coolTime,float damage,bool isPlayer)
    {
        Stats curStat;
        switch(isPlayer)
        {
            case true:
                playerStat= new Stats();
                playerStat.setStats(speed,hp,coolTime,damage);
                playerStat.addtionAbility();
                curStat = playerStat;
                curCharacter = player;           
                break;
            case false:
                enemyStat = new Stats();
                enemyStat.setStats(speed, hp, coolTime, damage);
                enemyStat.addtionAbility();
                curStat = enemyStat;
                curCharacter = enemy;
                break;
        }

        curCharacter.GetComponent<Character>().SetStat(curStat);

        Health health = null;
      
        if (curCharacter.TryGetComponent<Health>(out Health _health))
            health = _health;
        if(health != null) health.SetHp(hp);

    }

   
}
