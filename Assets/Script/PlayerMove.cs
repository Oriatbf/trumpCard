using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class PlayerMove : Character
{
    [Tab("Input")]
    [SerializeField] private LayerMask enemyMask;
  

    [Tab("Debug")]
    [SerializeField] Vector3 angleVec;

    private Camera _camera;
    Vector3 bowDir;


    [Tab("Mobile")]
    [SerializeField] bool mobileVersion;
    [SerializeField] VariableJoystick moveJoyStick,dirJoyStick;
    [SerializeField] Button dashBtn;
    Image dashBtnImage;


    public override void Awake()
    {
        base.Awake();
       
    }

    public override void Start()
    {
       

        base.Start();
        opponent = GameObject.FindWithTag("Enemy").transform;
        _camera = Camera.main;
        if(mobileVersion)
            dashBtnImage = dashBtn.GetComponent<Image>();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4)) Gambling();
        if (Input.GetKeyDown(KeyCode.F2)) health.GetDamage(1000);
        //if (Input.GetKeyDown(KeyCode.F9)) Debug.Log(characterSO.debuffs.Count);
    
        
        base.Update();

        if (Input.GetKeyDown(KeyCode.LeftShift) && curCharging > 0 && curDashCool <= 0)
        {
            Dash();
        }
        Move();

       // CoolTime(characterSO);
        if (mobileVersion)
        {
            if (curCharging > 0)
            {
                dashBtn.image.enabled = true;
                dashBtn.enabled = true;
            }
            else
            {
                dashBtn.image.enabled = false;
                dashBtn.enabled = false;
            }
        }
        

    
       

    }


    public void Dash()
    {
     
            if (curCharging > 0 && curDashCool <= 0)
            {
                DashMove(angleVec);
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

            angleVec = new Vector3(x, y, 0).normalized;

            float moveX = angleVec.x * stat.statValue.speed * Time.deltaTime;
            float moveY = angleVec.y * stat.statValue.speed * Time.deltaTime;
            transform.Translate(new Vector3(moveX, moveY, 0), Space.World);

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

            if (_dir.x < 0) handle.transform.localScale = new Vector3(-1, 1);
            if(_dir.x>0)handle.transform.localScale = new Vector3(1, 1);
    }

    

    public override void BowAttack()
    {
        if (!mobileVersion)
        {
            if (Input.GetMouseButton(0))
            {
                _curCharging += Time.deltaTime;
                if (_curCharging >= 1.5f) _curCharging = 1.5f;
                attackCoolImage.fillAmount = _curCharging / 1.5f;
            }

            if (Input.GetMouseButtonUp(0))
            {
                BowShoot();
            }
        }
        else
        {
           
            if(_dir != Vector3.zero) bowDir = _dir;
            if(_dir != Vector3.zero)
            {
                bowDir = _dir;
                _curCharging += Time.deltaTime;
                if (_curCharging >= stat.statValue.coolTime) _curCharging = stat.statValue.coolTime;
                attackCoolImage.fillAmount = _curCharging / stat.statValue.coolTime;
                if(_curCharging>=stat.statValue.coolTime)BowShoot();
               
            }

            if(_dir == Vector3.zero && _curCharging >= 0.3f)
            {
                BowShoot(bowDir);
            }
        }
       
    }
    

    public override void RangeAttack(bool isRevolver)
    {
        if (mobileVersion)
        {
            if( dirJoyStick.Direction != Vector2.zero) 
            {
                base.RangeAttack(isRevolver);
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                base.RangeAttack(isRevolver);
            }
        }
    }

    public override void MeleeAttack(bool isSting)
    {
        if (mobileVersion)
        {
            if ( dirJoyStick.Direction != Vector2.zero)
            {
                base.MeleeAttack(isSting);
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                base.MeleeAttack(isSting);
            }
        }
    }

    public override void MagicAttack()
    {

        if (mobileVersion)
        {
            if (dirJoyStick.Direction != Vector2.zero)
            {
                base.MagicAttack();
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                base.MagicAttack();
            }
        }
    }
}
