using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BowTest : MonoBehaviour
{
    public float maxCharging,curCharging;
    public GameObject bullet;
    public Image chargingImage;
    // Start is called before the first frame update
    void Start()
    {
        chargingImage.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            curCharging += Time.deltaTime;
            if (curCharging >= maxCharging) curCharging = maxCharging;
            chargingImage.fillAmount = curCharging / maxCharging;
        }

        if(Input.GetMouseButtonUp(0))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        chargingImage.fillAmount = 0;
        curCharging= 0;
        GameObject a= Instantiate(bullet,transform.position,Quaternion.identity);
        a.GetComponent<Rigidbody2D>().AddForce(Vector3.right * 10, ForceMode2D.Impulse);
    }
}
