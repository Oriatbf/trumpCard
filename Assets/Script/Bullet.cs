using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    public Rigidbody2D rigid;
    public float damage;
    Vector2 bulletDir;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {

        
        rigid.velocity = Vector2.zero;
        rigid.AddForce(bulletDir * 5f, ForceMode2D.Impulse);
        damage = PlayerStats.Inst.damage;

    }

    void OnEnable()
    {
        
        rigid.velocity = Vector2.zero;
        rigid.AddForce(bulletDir * 5f, ForceMode2D.Impulse);
        damage = PlayerStats.Inst.damage;
    }

    public void SetDir(Vector2 dir)
    {
        ActiveFalse();
        bulletDir = dir.normalized;
    }

  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Wall":
                gameObject.SetActive(false);
                break;
            case "Enemy":
                collision.gameObject.GetComponent<Health>().OnDamage(damage);
                gameObject.SetActive(false);
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
