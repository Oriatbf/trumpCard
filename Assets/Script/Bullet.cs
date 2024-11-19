using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInfor
{
    public float damage;
    public bool isReturn;
    public Vector2 dir;
    public float scale;
    public int bulletIndex;
    public bool isPlayerBullet;
    public List<Debuff> debuffs;
    public BulletInfor(float damage,bool isReturn, Vector2 bulletDir, float scale, int bulletIndex, bool isPlayerBullet, List<Debuff> debuffs)
    {
        this.damage = damage;
        this.isReturn = isReturn;
        this.dir = bulletDir;
        this.scale = scale;
        this.bulletIndex = bulletIndex;
        this.isPlayerBullet = isPlayerBullet;
        this.debuffs = debuffs;
    }
}

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

    public void SetDir(BulletInfor bulletInfor)
    {
        transform.localScale = new Vector3( bulletInfor.scale,bulletInfor.scale,bulletInfor.scale);
        bulletDir = bulletInfor.dir.normalized;
        isReturn = bulletInfor.isReturn;
        damage= bulletInfor.damage;
        debuffs = new List<Debuff>();
        debuffs = bulletInfor.debuffs;
        isPlayerBullet = bulletInfor.isPlayerBullet;
        for(int i = 0; i< transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        transform.GetChild(bulletInfor.bulletIndex).gameObject.SetActive(true);

        if (!isReturn)
            ActiveFalse();
        else
            Return(rigid);
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
