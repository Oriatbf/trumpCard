
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Map;
using static Attack;
using Newtonsoft.Json.Bson;

public class ShootInfor
{
    public int bulletCount;
    public int shootCount;
    public Vector3 dir;
    public CardStats charSO;
    public Transform curTrans;
    public Transform shootPoint;
    public bool isPlayer;
    public ShootInfor(Vector3 dir, CardStats charSO, bool isPlayer, Transform curTrans, Transform shootPoint)
    {
        bulletCount = charSO.infor.bulletCount;
        shootCount = charSO.infor.attackCount;
        this.isPlayer = isPlayer;
        this.dir = dir;
        this.curTrans = curTrans;
        this.shootPoint = shootPoint;
        this.charSO= charSO;
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
        Debug.Log(pool.gameObject);
    }


    public void QueenMagic(Transform shootPoint,CardStats charSO ,bool isPlayer,Transform target,Character character)
    { 
        for (int i = 0; i < charSO.infor.bulletCount; i++)
        {
            pool.magicBallPools[pool.magicIndex].transform.GetComponent<MagicBall>().SetTarget(target, Critical(charSO,charSO.infor.damage),isPlayer);
            pool.magicBallPools[pool.magicIndex].transform.position = shootPoint.position;
            pool.magicBallPools[pool.magicIndex].SetActive(true);
            
            pool.magicIndex++;
            if(pool.magicIndex > pool.magicBallPools.Length-1) pool.magicIndex = 0;
        }

    }


    
    public void Shoot(ShootInfor shootInfor,bool isMaxCharging = false)
    {
       
        if (pool.bulletIndex > pool.bulletPools.Length - 1) pool.bulletIndex = 0;
        float delay = 0;
        CardStats so = shootInfor.charSO;
        Vector2 finalDir = new Vector2(0,0);

        for (var j = 0; j < shootInfor.shootCount; j++)
        {
            DOVirtual.DelayedCall(delay, () =>
            {
                for (var i = 0; i < shootInfor.bulletCount; i++)
                {
                    float damage = shootInfor.charSO.infor.damage;
                    if (shootInfor.charSO.infor.attackType == CardStats.AttackType.Bow) //활 최대로 당겼을 때
                    {
                        damage = isMaxCharging ? so.infor.damage + so.infor.plusMaxChargeDam : so.infor.damage;
                    }

                    if (shootInfor.charSO.infor.attackType == CardStats.AttackType.ShotGun) // 샷건 해당
                    {
                        float angle = 50 * ((float)i / (shootInfor.bulletCount) - 0.5f); // Spread bullets evenly within the spreadAngle
                        finalDir = Quaternion.Euler(0, 0, angle) * shootInfor.dir; // Apply the angle to the direction vector
                    }
                    else finalDir = shootInfor.dir;

                    BulletInfor bulletInfor = new(Critical(so, so.infor.damage), so.infor.projectileTurnback, finalDir, so.relicInfor.size, so.infor.bulletTypeIndex, shootInfor.isPlayer,so.debuffs);
                    ProjectTileSet(shootInfor, bulletInfor);
                  
                }
            });
            delay = 0.2f + ((float)j / 10 - 0.1f);
        }
    }

    public void ProjectTileSet(ShootInfor shootInfor, BulletInfor bulletInfor)
    {
        Transform curBullet = pool.bulletPools[pool.bulletIndex].transform;
        pool.bulletPools[pool.bulletIndex].SetActive(true);

        // 총알 발사
        pool.bulletPools[pool.bulletIndex].transform.GetComponentInChildren<Bullet>().SetDir(bulletInfor);

        pool.bulletPools[pool.bulletIndex].transform.position = shootInfor.shootPoint.position;
        pool.bulletPools[pool.bulletIndex].transform.rotation = shootInfor.curTrans.rotation;

        //총알 회전
        Quaternion rotation = Quaternion.LookRotation(shootInfor.dir);
        curBullet.localScale = shootInfor.dir.x < 0 ? new Vector2(Mathf.Abs(curBullet.localScale.x) * -1, curBullet.localScale.y) : new Vector2(Mathf.Abs(curBullet.localScale.x), curBullet.localScale.y);
        rotation.x = 0;
        rotation.y = 0;
        curBullet.rotation = rotation;


        pool.bulletIndex++;

        isFlooring(shootInfor, curBullet);

        if (pool.bulletIndex > pool.bulletPools.Length - 1) pool.bulletIndex = 0;
    }

    public void Slash(ShootInfor shootInfor)
    {
        CardStats so = shootInfor.charSO;
        Vector2 finalDir = shootInfor.dir;
        BulletInfor bulletInfor = new(Critical(so, so.infor.damage*20/100), so.infor.projectileTurnback, finalDir, so.relicInfor.size, so.infor.bulletTypeIndex, shootInfor.isPlayer,so.debuffs);
        ProjectTileSet(shootInfor,bulletInfor);
    }

    void isFlooring(ShootInfor shootInfor, Transform curBullet)
    {
        if (shootInfor.charSO.relicInfor.isFlooring)
        {
            pool.f_bulletIndex++;
            pool.flooringPools[pool.f_bulletIndex].SetActive(true);
            pool.flooringPools[pool.f_bulletIndex].GetComponent<FlooringCol>().SetFloorObj(curBullet, shootInfor.charSO.relicInfor.floorTickDamage, shootInfor.isPlayer);
            if (pool.f_bulletIndex > pool.flooringPools.Length - 1) pool.f_bulletIndex = 0;
        }
    }

    public float Critical(CardStats charSO,float damage)
    {
        int a = Random.Range(1, 101);
        if (charSO.relicInfor.criticalChance >= a)
        {
            return damage * charSO.relicInfor.criticalDamage;
        }
        else return damage;
    }

   

}
