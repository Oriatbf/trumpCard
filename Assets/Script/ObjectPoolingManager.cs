using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager Inst;
    public GameObject bulletP,magicBallP;
    public GameObject[] bulletPools,magicBallPools;

    public Transform shootPoint;
    int index = 0;

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
        magicBallPools = new GameObject[3];
        for(int i = 0;i<magicBallPools.Length;i++)
        {    
            GameObject magicBall = Instantiate(magicBallP,magicBallParent.transform);
            magicBallPools[i] = magicBall;
            magicBall.SetActive(false);
        }
    }

    public void shootRevolver(Vector2 dir, Transform trans)
    {
        bulletPools[index].SetActive(true);
        bulletPools[index].transform.position = shootPoint.position;
        bulletPools[index].transform.rotation = trans.rotation;


        bulletPools[index].transform.GetChild(0).GetComponent<Bullet>().SetDir(dir);
        index++;
    }

    public void shootShotgun(Vector2 dir, Transform trans)
    {

        for(int i = 0; i < 5; i++)
        {
            float angle = 50 * ((float)i / (5) - 0.5f); // Spread bullets evenly within the spreadAngle
            Vector2 spreadDir = Quaternion.Euler(0, 0, angle) * dir; // Apply the angle to the direction vector

            // Activate and set up the bullet
            bulletPools[index].SetActive(true);
            bulletPools[index].transform.position = shootPoint.position;
            bulletPools[index].transform.rotation = trans.rotation;

 
            bulletPools[index].transform.GetChild(0).GetComponent<Bullet>().SetDir(spreadDir);
            index++;
        }

    }

    public void QueenMagic()
    {

        for(int i = 0; i < magicBallPools.Length; i++)
        {
            
            magicBallPools[i].transform.position = shootPoint.position;
            magicBallPools[i].SetActive(true);
        }
      
    }

    
}
