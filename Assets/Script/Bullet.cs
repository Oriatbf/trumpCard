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
    private List<StatusEffect> debuffs;
    Vector2 bulletDir;
    
    

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    

    public override void Init(Stat stat,Vector2 dir,CharacterType characterType,List<StatusEffect> _debuffs)
    {
        Set(stat,Critical.CriticalChance(stat),dir,characterType,_debuffs);
    }
    
    public override void Init(Stat stat, float _damage, Vector2 dir, CharacterType characterType,List<StatusEffect> _debuffs)
    {
        Set(stat,Critical.CriticalChance(stat,_damage),dir,characterType,_debuffs);
    }
    void Set(Stat stat, float _damage, Vector2 dir, CharacterType characterType,List<StatusEffect> _debuffs)
    {
        ownerCharacter = characterType;
        rigid.linearVelocity = Vector2.zero;
        debuffs = _debuffs;
        damage = _damage;
        bulletDir = dir;
        var _statValue = stat.FinalValue();
        bulletSpeed = _statValue.bulletSpeed;
        transform.localScale = new Vector3(_statValue.bulletSize, _statValue.bulletSize);
        ActiveFalse(2f);
    }
    

    private void Update()
    {
        rigid.linearVelocity = bulletDir * bulletSpeed*70 * Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Health health))
        {
            if (ownerCharacter != health.characterType)
            {
                health.GetDamage(damage);
                health.GetForce(bulletDir,5,0.05f);
                foreach (var debuff in debuffs)
                {
                    debuff.ApplyEffect(health);
                }
                EffectManager.Inst.SpawnEffect(transform, 0);
                ActiveFalse();
            }
        }
    }

}
