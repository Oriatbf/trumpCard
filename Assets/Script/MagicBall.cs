using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MagicBall : MonoBehaviour
{
    RandomBezier randomBezier;
    [SerializeField] float setRad, getRad;
    [SerializeField] Transform target;
    [SerializeField] float speed;
    private float t;

    // Start is called before the first frame update
    void Start()
    {
     
    }

    private void OnEnable()
    {
        target = GameObject.FindWithTag("Enemy").transform;
        randomBezier = new RandomBezier(transform.position, target.position, setRad, getRad);
        t = 0;
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
            collision.GetComponent<Health>().OnDamage(PlayerStats.Inst.damage);
            gameObject.SetActive(false);
        }
    }
}
