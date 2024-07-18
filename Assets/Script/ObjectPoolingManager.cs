using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager Inst;
    public GameObject bulletP;
    public GameObject[] bulletPools;
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
    }

    public void shootRevolver(Vector2 dir, Transform trans)
    {
        bulletPools[index].SetActive(true);
        bulletPools[index].transform.position = shootPoint.position;
        bulletPools[index].transform.rotation = trans.rotation;

        print(bulletPools[index].transform.rotation);
        bulletPools[index].transform.GetChild(0).GetComponent<Bullet>().SetDir(dir);
        index++;
    }

    public void shootShotgun(Vector2 dir, Transform trans)
    {

        for(int i = 0; i < 5; i++)
        {
            bulletPools[index].SetActive(true);
            bulletPools[index].transform.position = shootPoint.position;
            float angle = (120);
            float angleRad1 = angle * Mathf.Deg2Rad;
            Vector2 shootDir = new Vector2(Mathf.Cos(angleRad1), Mathf.Sin(angleRad1)).normalized;
            bulletPools[index].transform.GetChild(0).GetComponent<Bullet>().SetDir(shootDir);
            index++;
        }
;

    }

    
}
