using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class PlayerMove : Character
{
    [Tab("Input")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private CardStats card;
  

    [Tab("Debug")]
    [SerializeField] Vector3 angleVec;

    private float _angle,_curCharging;

    
    private Camera _camera;

   

    [Tab("Mobile")]
    [SerializeField] bool mobileVersion;
    [SerializeField] VariableJoystick moveJoyStick,dirJoyStick;


    public override void Start()
    {
      
      
        opponent = GameObject.FindWithTag("Enemy").transform;
        _camera = Camera.main;
        TypeManager.Inst.TypeChange(card.infor.cardNum, transform, true, characterSO);
        SetStat();

        base.Start();

        
    }

    // Update is called once per frame
    public override void Update()
    {
        if (GameManager.Inst.isGameStart)
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.LeftShift) && curCharging > 0 && curDashCool <= 0)
            {
                DashMove(angleVec);
            }


            Move();

            Gambling();

            if (isFlooring) FlooringDamage();

            CoolTime(characterSO);
        }
       

    }

    private void Gambling()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TypeManager.Inst.TypeChange(GambleManager.GambleIndex(),transform,true,characterSO);
            SetStat();
        }
    }

    private void Move()
    {
        float x;
        float y;
        //Move
        if (mobileVersion)
        {
             x = moveJoyStick.Horizontal;
             y = moveJoyStick.Vertical;
        }
        else
        {
             x = Input.GetAxisRaw("Horizontal");
             y = Input.GetAxisRaw("Vertical");
        }
      

        angleVec = new Vector3(x, y,0).normalized;

        float moveX = angleVec.x * speed * Time.deltaTime;
        float moveY = angleVec.y* speed * Time.deltaTime;
        transform.Translate(new Vector3(moveX,moveY,0),Space.World);
        
        //Rotation
        Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        if (mobileVersion)
        {
            _dir = new Vector3(dirJoyStick.Horizontal, dirJoyStick.Vertical, 0).normalized;
        }
        else
        {
            _dir = (mousePosition - transform.position).normalized;
        }
        
        _angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
    
        handle.transform.parent.rotation = Quaternion.Euler(0, 0, _dir.x < 0 ? _angle + 180 : _angle);
        handle.transform.localScale = new Vector3(_dir.x<0?-1:1, 1);
     
       
    }

    public override void BowAttack()
    {
        if (Input.GetMouseButton(0))
        {
            _curCharging += Time.deltaTime;
            if (_curCharging >= 1.5f) _curCharging = 1.5f;
            attackCoolImage.fillAmount = _curCharging / 1.5f;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        bool maxCharging = _curCharging >= 1.5f;
        Attack.Inst.shootBow(_dir, handle.transform.parent, shootPoint, characterSO, true,maxCharging);
        attackCoolImage.fillAmount = 0;
        _curCharging = 0;
       
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
        hit = Physics2D.Raycast(transform.position,_dir, 2f, enemyMask);
        if(hit.collider != null)
        {
            hit.transform.GetComponent<Health>().OnDamage(characterSO.infor.damage);
        }
    }


   

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawLine(transform.position,transform.position + _dir*1.5f);
        Gizmos.DrawWireCube(transform.position + transform.forward,transform.forward);
        Gizmos.DrawRay(transform.position, _dir*2);
    }

  

    


}
