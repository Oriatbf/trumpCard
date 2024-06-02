using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rigid;
    public float damage;
    Vector2 bulletDir;
    
    // Start is called before the first frame update
    void Start()
    {

        rigid= GetComponent<Rigidbody2D>();
        rigid.AddForce(bulletDir * 5f, ForceMode2D.Impulse);

    }

    public void SetDir(Vector2 dir)
    {
        bulletDir = dir.normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.collider.tag)
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
