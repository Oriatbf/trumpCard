using System;
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

    public void shootRevolver(Vector2 dir, Transform curTrans,Transform shootPoint,int bulletCount,bool isPlayer)
    {
        float delay = 0;
        for (var i = 0; i < bulletCount; i++)
        {
            DOVirtual.DelayedCall(delay, ()=>
            {
                pool.bulletPools[pool.bulletIndex].SetActive(true);
                pool.bulletPools[pool.bulletIndex].transform.position = shootPoint.position;
                pool.bulletPools[pool.bulletIndex].transform.rotation = curTrans.rotation;
                pool.bulletPools[pool.bulletIndex].transform.GetComponent<Bullet>().SetDir(dir);
                pool.bulletIndex++;
                if (pool.bulletIndex > pool.bulletPools.Length - 1) pool.bulletIndex = 0;
            });
            delay = 0.2f;

        }
       
    }

    public void shootShotgun(Vector2 dir, Transform curTrans, Transform shootPoint,int bulletCount,bool isPlayer)
    {

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = 50 * ((float)i / (bulletCount) - 0.5f); // Spread bullets evenly within the spreadAngle
            Vector2 spreadDir = Quaternion.Euler(0, 0, angle) * dir; // Apply the angle to the direction vector

            // Activate and set up the bullet
            pool.bulletPools[pool.bulletIndex].transform.GetComponent<Bullet>().SetDir(spreadDir);
            pool.bulletPools[pool.bulletIndex].SetActive(true);
            pool.bulletPools[pool.bulletIndex].transform.position = shootPoint.position;
            pool.bulletPools[pool.bulletIndex].transform.rotation = curTrans.rotation;
                

           
            pool.bulletIndex++;
            if (pool.bulletIndex > pool.bulletPools.Length - 1) pool.bulletIndex = 0;
        }

    }

    public void QueenMagic(Transform shootPoint,int bulletCount ,bool isPlayer,Transform target)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            pool.magicBallPools[pool.magicIndex].transform.GetComponent<MagicBall>().SetTarget(target);
            pool.magicBallPools[pool.magicIndex].transform.position = shootPoint.position;
            pool.magicBallPools[pool.magicIndex].SetActive(true);
            
            pool.magicIndex++;
            if(pool.magicIndex > pool.magicBallPools.Length-1) pool.magicIndex = 0;
        }

    }

}
