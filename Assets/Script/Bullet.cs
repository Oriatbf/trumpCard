using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : Projectile
{
    public Rigidbody2D rigid;
    public float damage;
    public bool isReturn;
    public List<Debuff> debuffs;
    Vector2 bulletDir;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rigid.linearVelocity = Vector2.zero;
        rigid.AddForce(bulletDir * 8f, ForceMode2D.Impulse);
       
       
    }


    void OnEnable()
    {
        
        rigid.linearVelocity = Vector2.zero;

        rigid.AddForce(bulletDir * 8f, ForceMode2D.Impulse);

    }

    public void Init()
    {
        
    }

  

    private void OnTriggerEnter2D(Collider2D collision)
    {
      
        if (collision.TryGetComponent<Health>(out Health health))
        {
            bool isOpponent = false;
            switch (collision.tag)
            {
                case "Enemy":
                    if (isPlayerBullet)
                    {
                        isOpponent = true;
                    }
                    break;
                case "Player":
                    if (!isPlayerBullet)
                    {
                        isOpponent= true;
                    }
                    break;
            }
            if (isOpponent)
            {
                foreach(Debuff debuff in debuffs)
                {
                    debuff.Apply(health);
                }
                health.OnDamage(damage);
             
                EffectManager.Inst.SpawnEffect(transform, 0);
                gameObject.SetActive(isReturn);
            }
           
        }
        


    }

}
