using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    public Rigidbody2D rigid;
    public float damage;
    [SerializeField] bool isPlayerBullet,isReturn;
    Vector2 bulletDir;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rigid.velocity = Vector2.zero;
        rigid.AddForce(bulletDir * 5f, ForceMode2D.Impulse);
       
        Debug.Log(transform.gameObject.activeSelf);
    }


    void OnEnable()
    {
        
        rigid.velocity = Vector2.zero;
       

        
    }

    public void SetDir(Vector2 dir,bool isPlayerBullet,bool isReturn,float damage,float scale)
    {
       transform.localScale *= scale;

        bulletDir = dir.normalized;
        this.damage= damage;
        this.isPlayerBullet = isPlayerBullet;
        rigid.AddForce(bulletDir * 5f, ForceMode2D.Impulse);
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
                gameObject.SetActive(false);
                break;
            case "Enemy":
                if (isPlayerBullet)
                {
                    collision.gameObject.GetComponent<Health>().OnDamage(damage);
                    gameObject.SetActive(false);
                }
                break;
            case "Player":
                if (!isPlayerBullet)
                {
                    collision.gameObject.GetComponent<Health>().OnDamage(damage);
                    gameObject.SetActive(false);
                }
                break;


        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
