
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    public void shootRevolver(Vector2 dir, Transform curTrans,Transform shootPoint,CardStats charSO,bool isPlayer,Character character)
    {
        float delay = 0;
        for (var i = 0; i < charSO.infor.attackCount; i++)
        {
            DOVirtual.DelayedCall(delay, ()=>
            {
                Transform curBullet = pool.bulletPools[pool.bulletIndex].transform;
                curBullet.GetComponent<Bullet>().SetDir(dir, isPlayer, charSO.infor.projectileTurnback, Critical( charSO, charSO.infor.damage * character.extraAttackRatio), charSO.relicInfor.size,charSO.infor.bulletTypeIndex);
                curBullet.gameObject.SetActive(true);
                curBullet.position = shootPoint.position;
                Quaternion rotation = Quaternion.LookRotation(dir);
                curBullet.localScale = dir.x < 0 ? new Vector2(Mathf.Abs(curBullet.localScale.x) * -1, curBullet.localScale.y) : new Vector2(Mathf.Abs(curBullet.localScale.x), curBullet.localScale.y);
                rotation.x = 0;
                rotation.y = 0;
                curBullet.rotation = rotation;
               
                pool.bulletIndex++;
                if (charSO.relicInfor.isFlooring)
                {
                     pool.f_bulletIndex++;
                     pool.flooringPools[pool.f_bulletIndex].SetActive(true);
                     pool.flooringPools[pool.f_bulletIndex].GetComponent<FlooringCol>().SetFloorObj(curBullet,charSO.relicInfor.floorTickDamage,isPlayer);
                     if (pool.f_bulletIndex > pool.flooringPools.Length - 1) pool.f_bulletIndex = 0;
                }
          
               
                if (pool.bulletIndex > pool.bulletPools.Length - 1) pool.bulletIndex = 0;
               
            });

            AudioManager.Inst.AudioEffectPlay(charSO.infor.cardNum);
            delay = 0.2f + ((float)i / 10 - 0.1f);

        }
       
    }

    public void shootBow(Vector2 dir, Transform curTrans, Transform shootPoint, CardStats charSO, bool isPlayer,bool isMaxCharge,Character character)
    {
        float delay = 0;
        float damage = isMaxCharge ?  charSO.infor.maxChargeDam :charSO.infor.damage;
        for (var i = 0; i < charSO.infor.attackCount; i++)
        {
            DOVirtual.DelayedCall(delay, () =>
            {
                Transform curBullet = pool.bulletPools[pool.bulletIndex].transform;
                curBullet.GetComponent<Bullet>().SetDir(dir, isPlayer, charSO.infor.projectileTurnback, Critical(charSO, damage*character.extraAttackRatio), charSO.relicInfor.size, charSO.infor.bulletTypeIndex);
                curBullet.gameObject.SetActive(true);
                curBullet.position = shootPoint.position;
              
                Quaternion rotation = Quaternion.LookRotation(dir);
                curBullet.localScale= dir.x < 0? new Vector2(Mathf.Abs(curBullet.localScale.x)*-1,curBullet.localScale.y): new Vector2(Mathf.Abs(curBullet.localScale.x), curBullet.localScale.y);
                rotation.x = 0;
                rotation.y = 0;
                
                curBullet.rotation = rotation;
               
                pool.bulletIndex++;
                if (pool.bulletIndex > pool.bulletPools.Length - 1) pool.bulletIndex = 0;
            });
            delay = 0.2f + ((float)i / 10 - 0.1f);

        }
        AudioManager.Inst.AudioEffectPlay(charSO.infor.cardNum);
    }

    public void shootShotgun(Vector2 dir, Transform curTrans, Transform shootPoint,CardStats charSO,bool isPlayer, Character character)
    {
        float delay = 0;
        for(int j = 0; j < charSO.infor.attackCount; j++)
        {
            DOVirtual.DelayedCall(delay, () =>
            {
                for (int i = 0; i < charSO.infor.bulletCount; i++)
                {
                    float angle = 50 * ((float)i / (charSO.infor.bulletCount) - 0.5f); // Spread bullets evenly within the spreadAngle
                    Vector2 spreadDir = Quaternion.Euler(0, 0, angle) * dir; // Apply the angle to the direction vector

                    // Activate and set up the bullet
                    pool.bulletPools[pool.bulletIndex].transform.GetComponent<Bullet>().SetDir(spreadDir, isPlayer, charSO.infor.projectileTurnback, Critical(charSO, charSO.infor.damage*character.extraAttackRatio), charSO.relicInfor.size,charSO.infor.bulletTypeIndex);

                    pool.bulletPools[pool.bulletIndex].SetActive(true);
                    pool.bulletPools[pool.bulletIndex].transform.position = shootPoint.position;
                    pool.bulletPools[pool.bulletIndex].transform.rotation = curTrans.rotation;



                    pool.bulletIndex++;
                    if (pool.bulletIndex > pool.bulletPools.Length - 1) pool.bulletIndex = 0;
                }
            });
            delay = 0.2f + ((float)j / 10 - 0.1f);
            AudioManager.Inst.AudioEffectPlay(charSO.infor.cardNum);
        }
      

    }

    public void QueenMagic(Transform shootPoint,CardStats charSO ,bool isPlayer,Transform target,Character character)
    { 
        for (int i = 0; i < charSO.infor.bulletCount; i++)
        {
            pool.magicBallPools[pool.magicIndex].transform.GetComponent<MagicBall>().SetTarget(target, Critical(charSO,charSO.infor.damage * character.extraAttackRatio),isPlayer);
            pool.magicBallPools[pool.magicIndex].transform.position = shootPoint.position;
            pool.magicBallPools[pool.magicIndex].SetActive(true);
            
            pool.magicIndex++;
            if(pool.magicIndex > pool.magicBallPools.Length-1) pool.magicIndex = 0;
        }

    }

    public void KingMagic(Vector2 dir, Transform curTrans, Transform shootPoint, CardStats charSO, bool isPlayer, Character character)
    {
        float delay = 0;
        for (var i = 0; i < charSO.infor.bulletCount; i++)
        {
            DOVirtual.DelayedCall(delay, () =>
            {
                pool.bulletPools[pool.bulletIndex].SetActive(true);
                pool.bulletPools[pool.bulletIndex].transform.position = shootPoint.position;
                pool.bulletPools[pool.bulletIndex].transform.rotation = curTrans.rotation;
                pool.bulletPools[pool.bulletIndex].transform.GetComponent<Bullet>().SetDir(dir, isPlayer, charSO.infor.projectileTurnback, Critical(charSO,charSO.infor.damage * character.extraAttackRatio),1,0);
                pool.bulletIndex++;
                if (pool.bulletIndex > pool.bulletPools.Length - 1) pool.bulletIndex = 0;
            });
            delay = 0.2f + ((float)i/10 - 0.1f);

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
