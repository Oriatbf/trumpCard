using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MagicBall : Projectile
{
    RandomBezier randomBezier;
    [SerializeField] float setRad, getRad;
    public float damage;
    [SerializeField] float speed;
    private float t;
    Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {

        t = 0;
        if(target != null)
            randomBezier = new RandomBezier(transform.position, target.position, setRad, getRad);
    }

    public void SetTarget(Transform transform,float damage,bool isPlayer)
    {
        isPlayerBullet= isPlayer;
        this.damage= damage;
        target = transform;
 
       
        ActiveFalse();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Bezier.GetPoint(randomBezier, t);
        if (t>=1)
        {
            gameObject.SetActive(false);
        }
        t += Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && isPlayerBullet)
        {
            collision.GetComponent<Health>().OnDamage(damage);
            gameObject.SetActive(false);
        }

        if (collision.CompareTag("Player") && !isPlayerBullet)
        {
            collision.GetComponent<Health>().OnDamage(damage);
            gameObject.SetActive(false);
        }
    }
}
