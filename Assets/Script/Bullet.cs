using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : Projectile
{
    public Rigidbody2D rigid;
    public float damage;
    public bool isReturn;
    private float bulletSpeed;
    Vector2 bulletDir;
    
    

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    

    public void Init(Stat stat,Vector2 dir,CharacterType characterType)
    {
        ownerCharacter = characterType;
        rigid.linearVelocity = Vector2.zero;
        damage = Critical.CriticalChance(stat);
        bulletDir = dir;
        bulletSpeed = stat.statValue.bulletSpeed;

        //rigid.AddForce(dir * stat.statValue.bulletSpeed*100*Time.deltaTime, ForceMode2D.Impulse);
    }

    private void Update()
    {
        rigid.linearVelocity = bulletDir * bulletSpeed*70 * Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Character character))
        {
            if (ownerCharacter != character.characterType)
            {
                character.health.GetDamage(damage);
                character.GetForce(bulletDir,5,0.05f);
                EffectManager.Inst.SpawnEffect(transform, 0);
                gameObject.SetActive(isReturn);
            }
        }
    }

}
