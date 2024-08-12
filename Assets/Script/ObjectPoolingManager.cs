using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager Inst;
    public GameObject bulletP,magicBallP,flooringBullet;
    public GameObject[] bulletPools,magicBallPools,flooringPools;

    public Transform shootPoint;
    public int bulletIndex = 0,magicIndex = 0,f_bulletIndex = 0;

    private void Awake()
    {
        Inst= this;
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject parentObj = new GameObject("PoolingParent");     //새로운 빈 오브젝트를 하나 만들어줌
        parentObj.transform.position= Vector3.zero;                 //새로운 빈 오브젝트의 위치 설정
        bulletPools = new GameObject[100];
        for(int i = 0;i<bulletPools.Length;i++)
        {
            GameObject bullet = Instantiate(bulletP,parentObj.transform);
            bulletPools[i] = bullet;
            bullet.SetActive(false);
        }

        GameObject magicBallParent = new GameObject("MBPoolingParent");
        magicBallParent.transform.position = Vector3.zero;
        magicBallPools = new GameObject[50];
        for(int i = 0;i<magicBallPools.Length;i++)
        {    
            GameObject magicBall = Instantiate(magicBallP,magicBallParent.transform);
            magicBallPools[i] = magicBall;
            magicBall.SetActive(false);
        }

        GameObject f_bulletParent = new GameObject("fb_PoolingParent");
        f_bulletParent.transform.position = Vector3.zero;
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
