using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : Projectile
{
    public Rigidbody2D rigid;
    public float damage;
    public bool isReturn;
    Vector2 bulletDir;
    
    CharacterType characterType;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    

    public void Init(Stat stat,Vector2 dir)
    {
        rigid.linearVelocity = Vector2.zero;
        damage = stat.damage;
        rigid.AddForce(dir * stat.bulletSpeed, ForceMode2D.Impulse);
        //Dir,damage,speed
    }

  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Character character))
        {
            if (characterType != character.characterType)
            {
                character.health.OnDamage(damage);
                EffectManager.Inst.SpawnEffect(transform, 0);
                gameObject.SetActive(isReturn);
            }
        }
    }

}
