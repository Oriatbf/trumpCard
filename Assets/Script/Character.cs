using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
     public float speed;
    public float coolTime, curCoolTime;
    public Image attackCoolImage;
    public GameObject[] weapons;
    public GameObject handle;
    public Transform shootPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetStat(CharacterStats.Stats selfStats)
    {
        speed = selfStats.finalSpeed;
        coolTime= selfStats.finalCoolTime;
        Debug.Log(speed);
    }

    public void SetWeapon(int index)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
            if (i == index)
            {
                weapons[i].SetActive(true);
            }
        }
    }
}
