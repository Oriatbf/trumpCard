using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : Character
{
    public RelicSkills relicSkills;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private CardStats card;
    [SerializeField] float maxCharging,dashSpeed;
    private float _angle,_curCharging;

    Rigidbody2D rigid;
    DashEffect dashEffect;

    private Camera _camera;

    [SerializeField] Vector3 angleVec;

    [Header ("모바일 조이스틱")]
    [SerializeField] bool mobileVersion;
    [SerializeField] VariableJoystick moveJoyStick,dirJoyStick;


    private void Start()
    {
        dashEffect= GetComponent<DashEffect>();
        rigid = GetComponent<Rigidbody2D>();
        opponent = GameObject.FindWithTag("Enemy").transform;
        _camera = Camera.main;
        TypeManager.Inst.TypeChange(card.infor.cardNum, transform, true, characterSO);
        relicSkills = GetComponent<RelicSkills>();
        relicSkills.StartSkill();
        relicSkills.StartRatioSkill();

        SetStat();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            health.OnDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            DashMove();
        }

        relicSkills.MovingSkill();

        Move();

        Gambling();

        if (isFlooring) FlooringDamage();

       CoolTime(characterSO);

    }

    private void Gambling()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TypeManager.Inst.TypeChange(GambleManager.GambleIndex(),transform,true,characterSO);
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
        transform.rotation = Quaternion.Euler(0, 0, _dir.x < 0 ? _angle + 180 : _angle);
        handle.transform.localScale = new Vector3(_dir.x<0?-1:1, 1);
     
       
    }

    public override void BowAttack()
    {
        if (Input.GetMouseButton(0))
        {
            _curCharging += Time.deltaTime;
            if (_curCharging >= maxCharging) _curCharging = maxCharging;
            attackCoolImage.fillAmount = _curCharging / maxCharging;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        attackCoolImage.fillAmount = 0;
        _curCharging = 0;
        Attack.Inst.shootBow(_dir, transform, shootPoint, characterSO, true);
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


    public void DashMove()
    {
        rigid.velocity = new Vector2(angleVec.x,angleVec.y) *dashSpeed;
        dashEffect.ActiveDashEffect(0.2f);
        DOVirtual.DelayedCall(0.2f, () => rigid.velocity = Vector2.zero) ;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawLine(transform.position,transform.position + _dir*1.5f);
        Gizmos.DrawWireCube(transform.position + transform.forward,transform.forward);
        Gizmos.DrawRay(transform.position, _dir*2);
    }

  

    


}
