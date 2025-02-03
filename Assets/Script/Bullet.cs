using System;
using System.Collections;
using System.Collections.Generic;
using OneLine.Examples;
using UnityEngine;


public class Bullet : Projectile
{
    public float damage;
    private float bulletSpeed;
    private List<StatusEffect> debuffs;
    Vector2 bulletDir;
    [SerializeField]private List<GameObject> bulletType;
    
    


    

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
        SetBulletType(stat.bulletIndex);
        debuffs = _debuffs;
        damage = _damage;
        bulletDir = dir;
        var _statValue = stat.FinalValue();
        bulletSpeed = _statValue.bulletSpeed;
        transform.localScale = new Vector3(_statValue.bulletSize, _statValue.bulletSize);
        ActiveFalseTimer(2f,stat.cardNum == 4);
    }

    private void SetBulletType(int index)
    {
        foreach (var b in bulletType)
        {
            b.SetActive(false);
        }
        
        bulletType[index].SetActive(true);
    }

    private void Update()
    {
        rigid.linearVelocity = bulletDir * bulletSpeed*70 * RigidVelocity() * Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Health health))
        {
            if (ownerCharacter != health.characterType)
            {
                health.GetDamage(damage);
                health.GetForce(bulletDir,5,0.05f);
                if (debuffs.Count > 0 )
                {
                    foreach (var debuff in debuffs)
                    {
                        debuff.ApplyEffect(health);
                    }
                }
                
                EffectManager.Inst.SpawnEffect(transform, 0);
                ActiveFalseTimer(0f);
            }
        }
    }

}
