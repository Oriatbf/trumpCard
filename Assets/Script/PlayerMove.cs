using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : Character
{
 
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private CardStats card;
    
    private float _angle;
    


    private Camera _camera;


   
    private void Start()
    {
        
        opponent = GameObject.FindWithTag("Enemy").transform;
        _camera = Camera.main;
        TypeManager.Inst.TypeChange(card.cardNum,transform,true);
        
       // SetStat();
      
    }

    // Update is called once per frame
    void Update()
    {
        // 이거 공통으로 바꾸기

        Move();
        Gambling();

       CoolTime(TypeManager.Inst.playerCurSO);

    }

    private void Gambling()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TypeManager.Inst.TypeChange(GambleManager.GambleIndex(),transform,true);
        }
    }

    private void Move()
    {
        //Move
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 moveDir = new Vector2(x, y).normalized;

        float moveX = moveDir.x * speed * Time.deltaTime;
        float moveY = moveDir.y* speed * Time.deltaTime;
        transform.Translate(new Vector3(moveX,moveY,0),Space.World);
        
        //Rotation
        Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        _dir = (mousePosition - transform.position).normalized;
        _angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, _dir.x < 0 ? _angle + 180 : _angle);
        handle.transform.localScale = new Vector3(_dir.x<0?-1:1, 1);
     
       
    }


    public override void RangeAttack(bool isRevolver, CardStats curSO)
    {
        if(Input.GetMouseButton(0))
        {
            base.RangeAttack(isRevolver, curSO);
        }
       
    }

    public override void MagicAttack(CardStats curSO)
    {
        if (Input.GetMouseButton(0))
        {
            base.MagicAttack(curSO);
        }
       
    }



    public void MeleeDamage()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, transform.right, 2f, enemyMask);
        if(hit.collider != null)
        {
            hit.transform.GetComponent<Health>().OnDamage(CharacterStats.Inst.damage);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,transform.position + _dir*1.5f);
        Gizmos.DrawWireCube(transform.position + transform.forward,transform.forward);
        Gizmos.DrawRay(transform.position, transform.right * 2);
    }

  

    


}
