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
    

    public void Init(Stat stat,Vector2 dir,CharacterType characterType)
    {
        ownerCharacter = characterType;
        rigid.linearVelocity = Vector2.zero;
        damage = stat.damage;
        rigid.AddForce(dir * stat.bulletSpeed, ForceMode2D.Impulse);
    }

  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Character character))
        {
            if (ownerCharacter != character.characterType)
            {
                character.health.GetDamage(damage);
                EffectManager.Inst.SpawnEffect(transform, 0);
                gameObject.SetActive(isReturn);
            }
        }
    }

}
