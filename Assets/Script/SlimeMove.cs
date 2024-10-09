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
    bool isPlayer;
    SpriteRenderer spr;
    // Start is called before the first frame update
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    public void SetInfor(bool isPlayer)
    {
        this.isPlayer = isPlayer;
        if (isPlayer) opponent = GameObject.FindGameObjectWithTag("Enemy").transform;
        else opponent = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Inst.isGameStart && opponent != null)
        {
            if (opponent.position.x < transform.position.x) spr.flipX = false;
            else spr.flipX = true;
            Vector3 dir = (opponent.position - transform.position).normalized;
            transform.eulerAngles = dir;
            transform.Translate(dir * speed * Time.deltaTime);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayer)
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<Health>().OnDamage(5);
                GameObject floorObj = Instantiate(floor, transform.position, Quaternion.identity);
                Destroy(floorObj, floorDestoryTime);
                Destroy(gameObject);
            }
        }
        else
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<Health>().OnDamage(5);
                GameObject floorObj = Instantiate(floor, transform.position, Quaternion.identity);
                Destroy(floorObj, floorDestoryTime);
                Destroy(gameObject);
            }
        }
        
    }
}
