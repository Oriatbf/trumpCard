
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Attack : MonoBehaviour
{
    public static Attack Inst;
    ObjectPoolingManager pool;
    private void Start()
    {
        pool = ObjectPoolingManager.Inst;
        Inst = this;
    }

    public void shootRevolver(Vector2 dir, Transform curTrans,Transform shootPoint,CardStats charSO,bool isPlayer)
    {
        float delay = 0;
        for (var i = 0; i < charSO.infor.attackCount; i++)
        {
            DOVirtual.DelayedCall(delay, ()=>
            {
                Transform bulletTrans = pool.bulletPools[pool.bulletIndex].transform;
                bulletTrans.GetComponent<Bullet>().SetDir(dir, isPlayer, charSO.infor.projectileTurnback, Critical( charSO), charSO.relicInfor.size);
                bulletTrans.gameObject.SetActive(true);
                bulletTrans.position = shootPoint.position;
                bulletTrans.rotation = curTrans.rotation;
               
                pool.bulletIndex++;
                if (charSO.relicInfor.isFlooring)
                {
                     pool.f_bulletIndex++;
                    pool.flooringPools[pool.f_bulletIndex].SetActive(true);
                    pool.flooringPools[pool.f_bulletIndex].GetComponent<FlooringCol>().SetFloorObj(bulletTrans,charSO.relicInfor.floorTickDamage,isPlayer);
                    if (pool.f_bulletIndex > pool.flooringPools.Length - 1) pool.f_bulletIndex = 0;
                }
          
               
                if (pool.bulletIndex > pool.bulletPools.Length - 1) pool.bulletIndex = 0;
               
            });
            delay = 0.2f + ((float)i / 10 - 0.1f);

        }
       
    }

    public void shootBow(Vector2 dir, Transform curTrans, Transform shootPoint, CardStats charSO, bool isPlayer)
    {
        float delay = 0;
        for (var i = 0; i < charSO.infor.attackCount; i++)
        {
            DOVirtual.DelayedCall(delay, () =>
            {
                pool.bulletPools[pool.bulletIndex].transform.GetComponent<Bullet>().SetDir(dir, isPlayer, charSO.infor.projectileTurnback, Critical(charSO), charSO.relicInfor.size);
                pool.bulletPools[pool.bulletIndex].SetActive(true);
                pool.bulletPools[pool.bulletIndex].transform.position = shootPoint.position;
                pool.bulletPools[pool.bulletIndex].transform.rotation = curTrans.rotation;
               
                pool.bulletIndex++;
                if (pool.bulletIndex > pool.bulletPools.Length - 1) pool.bulletIndex = 0;
            });
            delay = 0.2f + ((float)i / 10 - 0.1f);

        }
    }

    public void shootShotgun(Vector2 dir, Transform curTrans, Transform shootPoint,CardStats charSO,bool isPlayer)
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
                    pool.bulletPools[pool.bulletIndex].transform.GetComponent<Bullet>().SetDir(spreadDir, isPlayer, charSO.infor.projectileTurnback, Critical(charSO), charSO.relicInfor.size);

                    pool.bulletPools[pool.bulletIndex].SetActive(true);
                    pool.bulletPools[pool.bulletIndex].transform.position = shootPoint.position;
                    pool.bulletPools[pool.bulletIndex].transform.rotation = curTrans.rotation;



                    pool.bulletIndex++;
                    if (pool.bulletIndex > pool.bulletPools.Length - 1) pool.bulletIndex = 0;
                }
            });
            delay = 0.2f + ((float)j / 10 - 0.1f);

        }
      

    }

    public void QueenMagic(Transform shootPoint,CardStats charSO ,bool isPlayer,Transform target)
    {
        for (int i = 0; i < charSO.infor.bulletCount; i++)
        {
            pool.magicBallPools[pool.magicIndex].transform.GetComponent<MagicBall>().SetTarget(target, Critical(charSO));
            pool.magicBallPools[pool.magicIndex].transform.position = shootPoint.position;
            pool.magicBallPools[pool.magicIndex].SetActive(true);
            
            pool.magicIndex++;
            if(pool.magicIndex > pool.magicBallPools.Length-1) pool.magicIndex = 0;
        }

    }

    public void KingMagic(Vector2 dir, Transform curTrans, Transform shootPoint, CardStats charSO, bool isPlayer)
    {
        float delay = 0;
        for (var i = 0; i < charSO.infor.bulletCount; i++)
        {
            DOVirtual.DelayedCall(delay, () =>
            {
                pool.bulletPools[pool.bulletIndex].SetActive(true);
                pool.bulletPools[pool.bulletIndex].transform.position = shootPoint.position;
                pool.bulletPools[pool.bulletIndex].transform.rotation = curTrans.rotation;
                pool.bulletPools[pool.bulletIndex].transform.GetComponent<Bullet>().SetDir(dir, isPlayer, charSO.infor.projectileTurnback, Critical(charSO),1);
                pool.bulletIndex++;
                if (pool.bulletIndex > pool.bulletPools.Length - 1) pool.bulletIndex = 0;
            });
            delay = 0.2f + ((float)i/10 - 0.1f);

        }
    }

    public float Critical(CardStats charSO)
    {
        int a = Random.Range(1, 101);
        if(charSO.relicInfor.ciritical >= a)
        {
            return charSO.infor.damage * 2;
        }
        else return charSO.infor.damage;
    }

   

}
