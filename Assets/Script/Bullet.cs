using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    public Rigidbody2D rigid;
    public float damage;
    public bool isReturn;
    Vector2 bulletDir;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rigid.velocity = Vector2.zero;
        rigid.AddForce(bulletDir * 5f, ForceMode2D.Impulse);
       
       
    }


    void OnEnable()
    {
        
        rigid.velocity = Vector2.zero;

        rigid.AddForce(bulletDir * 5f, ForceMode2D.Impulse);

    }

    public void SetDir(Vector2 dir,bool isPlayerBullet,bool isReturn,float damage,float scale,int bulletIndex)
    {
       transform.localScale *= scale;
        bulletDir = dir.normalized;
        this.isReturn = isReturn;
        this.damage= damage;
        this.isPlayerBullet = isPlayerBullet;
        for(int i = 0; i< transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        transform.GetChild(bulletIndex).gameObject.SetActive(true);
        if (!isReturn)
            ActiveFalse();
        else
            Return(rigid);
    }

  

    private void OnTriggerEnter2D(Collider2D collision)
    {

        switch (collision.tag)
        {
            case "Wall":
                gameObject.SetActive(isReturn);
                break;
            case "Enemy":
                if (isPlayerBullet)
                {
                    collision.gameObject.GetComponent<Health>().OnDamage(damage);
                    gameObject.SetActive(isReturn);
                }
                break;
            case "Player":
                if (!isPlayerBullet)
                {
                    collision.gameObject.GetComponent<Health>().OnDamage(damage);
                    gameObject.SetActive(isReturn);
                }
                break;


        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
