using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : Character
{

    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private CardStats card;
    [SerializeField] private float Gunrange;
    private float _angle;
    private Vector2 _dir2;

    private Camera _camera;

   
    
    void Start()
    {
        opponent = GameObject.FindGameObjectWithTag("Player").transform;
        _dir = (opponent.position - transform.position).normalized;
        _camera = Camera.main;
        TypeManager.Inst.TypeChange(card.cardNum, transform, false);
    }

    // Update is called once per frame
    void Update()
    {
       
        Move();
        Gambling();
        CoolTime(TypeManager.Inst.enemyCurSO);
    }
    private void Gambling()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TypeManager.Inst.TypeChange(GambleManager.GambleIndex(), transform, false);
        }
    }


    private void Move()
    {
        //회전
        Vector2 finalDir =  new Vector2(0,0);
        _angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, _dir.x < 0 ? _angle+180  : _angle);
        handle.transform.localScale = new Vector3(_dir.x<0?-1:1, 1);
        _dir = (opponent.position - transform.position).normalized;
        if ((opponent.position - transform.position).sqrMagnitude > Gunrange * Gunrange)
        {
           
            finalDir = _dir;
        }
        if ((opponent.position - transform.position).sqrMagnitude < (Gunrange-3) * (Gunrange-3))
        {
            _dir2 = (transform.position - opponent.position).normalized;
            finalDir = _dir2;
        }

        //이동
        float moveX = finalDir.x * speed * Time.deltaTime;
        float moveY = finalDir.y * speed * Time.deltaTime;
       
         transform.Translate(new Vector3(moveX,moveY,0),Space.World);
        
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,Gunrange);
        Gizmos.DrawWireSphere(transform.position, Gunrange-3);
    }



}
