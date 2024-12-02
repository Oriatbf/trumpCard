using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager Inst;
    public GameObject magicBallP,flooringBullet;
    public Bullet bulletP;
    public GameObject[] magicBallPools,flooringPools;
    public Bullet[] bulletPools;

    public int bulletIndex = 0,magicIndex = 0,f_bulletIndex = 0;

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
    // Start is called before the first frame update
    void Start()
    {
        GameStart();
    }

    private void GameStart()
    {
        GameObject parentObj = new GameObject("PoolingParent");     //새로운 빈 오브젝트를 하나 만들어줌
        parentObj.transform.position = Vector3.zero;                 //새로운 빈 오브젝트의 위치 설정
        parentObj.transform.SetParent(transform, true);
        bulletPools = new Bullet[200];
        for (int i = 0; i < bulletPools.Length; i++)
        {
            Bullet bullet = Instantiate(bulletP, parentObj.transform);
            bulletPools[i] = bullet;
            bullet.gameObject.SetActive(false);
        }

        GameObject magicBallParent = new GameObject("MBPoolingParent");
        magicBallParent.transform.position = Vector3.zero;
        magicBallParent.transform.SetParent(transform, true);
        magicBallPools = new GameObject[50];
        for (int i = 0; i < magicBallPools.Length; i++)
        {
            GameObject magicBall = Instantiate(magicBallP, magicBallParent.transform);
            magicBallPools[i] = magicBall;
            magicBall.SetActive(false);
        }

        GameObject f_bulletParent = new GameObject("fb_PoolingParent");
        f_bulletParent.transform.position = Vector3.zero;
        f_bulletParent.transform.SetParent(transform, true);
        flooringPools = new GameObject[100];
        for (int i = 0; i < flooringPools.Length; i++)
        {
            GameObject f_bullet = Instantiate(flooringBullet, f_bulletParent.transform);
            f_bullet.transform.position = Vector3.zero;
            flooringPools[i] = f_bullet;
            f_bullet.SetActive(false);
        }
    }

   

    
}
