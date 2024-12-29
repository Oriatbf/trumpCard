
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
        var stat = shootInfor.stat.FinalValue();
        float delay = 0;
        Vector2 finalDir = new Vector2(0,0);
        float spreadAngle = 30;
        float startAngle = -spreadAngle / 2f;
        float angleStep = spreadAngle / (stat.extraHitCount - 1);

        for (var j = 0; j < stat.attackCount; j++)
        {
            DOVirtual.DelayedCall(delay, () =>
            {
                for (var i = 0; i < stat.extraHitCount; i++)
                {
                    Quaternion rotation = Quaternion.Euler(0, 0, shootInfor.character._angle);;
                    if (shootInfor.stat.cardRole == CardRole.ShotGun) // 샷건 해당
                    {
                        float curAngle = startAngle + angleStep * i;
                        rotation = Quaternion.Euler(0, 0, shootInfor.character._angle + curAngle);
                        finalDir = rotation * Vector2.right;
                    }
                    else finalDir = shootInfor.dir;

                   
                    ProjectTileSet(shootInfor,rotation,finalDir);
                  
                }
            });
            delay = 0.2f + ((float)j / 10 - 0.1f);
        }
    }

    public void ProjectTileSet(ShootInfor shootInfor,Quaternion rot,Vector2 dir)
    {
        Projectile curBullet = pool.GetObjectFromPool("Bullet");
        curBullet.gameObject.SetActive(true);
        // 총알 발사
        curBullet.Init(shootInfor.stat,dir,shootInfor.characterType);
        

        curBullet.transform.position = shootInfor.shootPoint.position;
        curBullet.transform.rotation = rot;
        
        
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

    

   

}

public static class Critical
{
    public static float CriticalChance(Stat stat)
    {
        var _statValue = stat.FinalValue();
        int a = Random.Range(1, 101);
        float damage = _statValue.damage;
        if (_statValue.criticalChance >= a)
        {
            return damage * _statValue.criticalMultiplier;
        }
        else return damage;
    }
}
