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
    [SerializeField] VariableJoystick moveJoyStick,dirJoyStick;
    [SerializeField] Button dashBtn;
    Image dashBtnImage;


    public override void Awake()
    {
        base.Awake();
        moveJoyStick = JoyStickController.Inst.moveJoyStick;
        dirJoyStick = JoyStickController.Inst.dirJoyStick;
    }

    public override void Start()
    {
        base.Start();
        opponent = GameManager.Inst.GetOpponent(unitHealth.characterType);
        if (DataManager.Inst.Data.moblieVersion)
        {
            JoyStickController.Inst.interactionBtn.onClick.RemoveAllListeners();
            JoyStickController.Inst.interactionBtn.onClick.AddListener(()=>Dash());
        }
           
        _camera = Camera.main;
    }
    

    // Update is called once per frame
    public override void Update()
    {
        if(GameManager.Inst.Pause()) return;
        if (Input.GetKeyDown(KeyCode.F4)) Gambling();
        if (Input.GetKeyDown(KeyCode.F2)) unitHealth.GetDamage(1000);
        
        base.Update();

        if (Input.GetKeyDown(KeyCode.LeftShift) && curCharging > 0 && curDashCool <= 0)
        {
            Dash();
        }
        Move();
        
        if (DataManager.Inst.Data.moblieVersion)
        {
            if (curCharging > 0)
            {
                //dashBtn.image.enabled = true;
                //dashBtn.enabled = true;
            }
            else
            {
                //dashBtn.image.enabled = false;
               // dashBtn.enabled = false;
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
    protected override void Move()
    {
       
            float x;
            float y;
            //Move
            if (DataManager.Inst.Data.moblieVersion)
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

            var _statsValue = stat.FinalValue();
            float moveX = angleVec.x * _statsValue.speed * Time.deltaTime;
            float moveY = angleVec.y * _statsValue.speed * Time.deltaTime;
            transform.Translate(new Vector3(moveX, moveY, 0), Space.World);

            //Rotation
            Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            if (DataManager.Inst.Data.moblieVersion)
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

    

    protected override void BowAttack()
    {
        if (!DataManager.Inst.Data.moblieVersion)
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
                if (_curCharging >= stat.originStatValue.coolTime) _curCharging = stat.originStatValue.coolTime;
                attackCoolImage.fillAmount = _curCharging / stat.originStatValue.coolTime;
                if(_curCharging>=stat.originStatValue.coolTime)BowShoot();
               
            }

            if(_dir == Vector3.zero && _curCharging >= 0.3f)
            {
                BowShoot(bowDir);
            }
        }
       
    }
    

    public override void RangeAttack(bool isRevolver)
    {
        if (DataManager.Inst.Data.moblieVersion)
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
        if (DataManager.Inst.Data.moblieVersion)
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

        if (DataManager.Inst.Data.moblieVersion)
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
