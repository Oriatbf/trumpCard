using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MagicBall : Projectile
{
    RandomBezier randomBezier;
    [SerializeField] float setRad, getRad;
    public float damage;
    [SerializeField] Transform target;
    [SerializeField] float speed;
    private float t;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        if(GameObject.FindWithTag("Enemy") != null)
        {
            target = GameObject.FindWithTag("Enemy").transform;
            randomBezier = new RandomBezier(transform.position, target.position, setRad, getRad);
           
        }
        t = 0;
    }

    public void SetTarget(Transform transform,float damage)
    {
        this.damage= damage;
        target = transform;
        ActiveFalse();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Bezier.GetPoint(randomBezier, t);
        if (transform.position == target.position)
        {
            gameObject.SetActive(false);
        }
        t += Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Health>().OnDamage(damage);
            gameObject.SetActive(false);
        }
    }
}
