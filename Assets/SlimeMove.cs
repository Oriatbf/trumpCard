using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SlimeMove : MonoBehaviour
{
    [SerializeField] Transform opponent;
    [SerializeField] float speed,floorDestoryTime;
    [SerializeField] GameObject floor;
    // Start is called before the first frame update
    void Start()
    {
        opponent = GameObject.FindGameObjectWithTag("Enemy").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = (opponent.position - transform.position).normalized;
        transform.eulerAngles= dir;
        transform.Translate(dir*speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Health>().OnDamage(5);
            GameObject floorObj =  Instantiate(floor, transform.position,Quaternion.identity);
            Destroy(floorObj,floorDestoryTime);
            Destroy(gameObject);
        }
    }
}
