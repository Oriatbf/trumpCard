
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Map;
using static Attack;
using Newtonsoft.Json.Bson;

public class ShootInfor
{
    public Vector3 dir;
    public Transform curTrans;
    public Transform shootPoint;
    public Character character;
    public CharacterType characterType;
    public Stat stat;

    public ShootInfor(Character character,Stat stat)
    {
        this.dir = character._dir;
        this.curTrans = character.transform;
        this.shootPoint = character.shootPoint;
        this.character = character;
        characterType = character.characterType;
        this.stat = stat;
    }
}
public class Attack : MonoBehaviour
{
   

    public static Attack Inst;
    ObjectPoolingManager pool;

    private void Awake()
    {
        if (Inst != this && Inst != null)
        {
            return;
        }
        else
        {
            Inst = this;
        }
    }
    private void Start()
    {
        pool = ObjectPoolingManager.Inst;
    }

/*
    public void QueenMagic(ShootInfor shootInfor)
    { 
        for (int i = 0; i < shootInfor.stat.extraHitCount; i++)
        {
            pool.magicBallPools[pool.magicIndex].transform.GetComponent<MagicBall>().SetTarget(target, Critical(shootInfor.stat),isPlayer);
            pool.magicBallPools[pool.magicIndex].transform.position = shootInfor.character.shootPoint.position;
            pool.magicBallPools[pool.magicIndex].SetActive(true);
            
            pool.magicIndex++;
            if(pool.magicIndex > pool.magicBallPools.Length-1) pool.magicIndex = 0;
        }

    }*/


    
    public void Shoot(ShootInfor shootInfor,bool isMaxCharging = false)
    {
       
        if (pool.bulletIndex > pool.bulletPools.Length - 1) pool.bulletIndex = 0;
        float delay = 0;
        Vector2 finalDir = new Vector2(0,0);

        for (var j = 0; j < shootInfor.stat.attackCount; j++)
        {
            DOVirtual.DelayedCall(delay, () =>
            {
                for (var i = 0; i < shootInfor.stat.extraHitCount; i++)
                {
                    float damage = shootInfor.stat.damage;
                    
                    if (shootInfor.stat.cardRole == CardRole.Bow) //활 최대로 당겼을 때
                    {
                       // damage = isMaxCharging ? so.infor.damage + so.infor.plusMaxChargeDam : so.infor.damage;
                    }
                        
                    if (shootInfor.stat.cardRole == CardRole.ShotGun) // 샷건 해당
                    {
                        float angle = 50 * ((float)i / (shootInfor.stat.extraHitCount) - 0.5f); // Spread bullets evenly within the spreadAngle
                        finalDir = Quaternion.Euler(0, 0, angle) * shootInfor.dir; // Apply the angle to the direction vector
                    }
                    else finalDir = shootInfor.dir;

                   
                    ProjectTileSet(shootInfor);
                  
                }
            });
            delay = 0.2f + ((float)j / 10 - 0.1f);
        }
    }

    public void ProjectTileSet(ShootInfor shootInfor)
    {
        Bullet curBullet = pool.bulletPools[pool.bulletIndex];
        curBullet.gameObject.SetActive(true);
        // 총알 발사
        curBullet.Init(shootInfor.stat,shootInfor.dir,shootInfor.characterType);
        

        curBullet.transform.position = shootInfor.shootPoint.position;
        curBullet.transform.rotation = shootInfor.curTrans.rotation;

        //총알 회전
        Quaternion rotation = Quaternion.LookRotation(shootInfor.dir);
        curBullet.transform.localScale = 
            shootInfor.dir.x < 0 ? new Vector2(Mathf.Abs(curBullet.transform.localScale.x) * -1, curBullet.transform.localScale.y) 
                : new Vector2(Mathf.Abs(curBullet.transform.localScale.x), curBullet.transform.localScale.y);
        rotation.x = 0;
        rotation.y = 0;
        curBullet.transform.rotation = rotation;

        pool.bulletIndex++;
        if (pool.bulletIndex > pool.bulletPools.Length - 1) pool.bulletIndex = 0;
    }
/*
    public void Slash(ShootInfor shootInfor)
    {
        CardStats so = shootInfor.charSO;
        Vector2 finalDir = shootInfor.dir;
        BulletInfor bulletInfor = new(Critical(so, so.infor.damage*20/100), so.infor.projectileTurnback, finalDir, so.relicInfor.size, so.infor.bulletTypeIndex, shootInfor.isPlayer,so.debuffs);
        ProjectTileSet(shootInfor,bulletInfor);
    }*/

    /*
    void isFlooring(ShootInfor shootInfor, Transform curBullet)
    {
        if (shootInfor.charSO.relicInfor.isFlooring)
        {
            pool.f_bulletIndex++;
            pool.flooringPools[pool.f_bulletIndex].SetActive(true);
            pool.flooringPools[pool.f_bulletIndex].GetComponent<FlooringCol>().SetFloorObj(curBullet, shootInfor.charSO.relicInfor.floorTickDamage, shootInfor.isPlayer);
            if (pool.f_bulletIndex > pool.flooringPools.Length - 1) pool.f_bulletIndex = 0;
        }
    }*/

    public float Critical(Stat stat)
    {
        int a = Random.Range(1, 101);
        float damage = stat.damage;
        if (stat.criticalChance >= a)
        {
            return damage * stat.criticalMultiplier;
        }
        else return damage;
    }

   

}
