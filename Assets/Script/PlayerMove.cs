using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    [HideInInspector] public float speed;
    [HideInInspector]public float coolTime,curCoolTime;
    [SerializeField] private Image attackCoolImage;
    [SerializeField] private GameObject bullet;

    [SerializeField] private Transform shootPoint;
    [SerializeField] private LayerMask enemyMask;

    [SerializeField] private GameObject handle;
    [SerializeField] private CardStats card; 
    
    private float _angle;
    private Vector3 _dir;


    private Animator animator;
    private Camera _camera;

    private Transform opponent;

    private void Start()
    {
        opponent = GameObject.FindWithTag("Enemy").transform;
        _camera = Camera.main;
        TypeManager.Inst.TypeChange(card.cardNum,transform,true);
        animator = handle.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 이거 공통으로 바꾸기
        switch (TypeManager.Inst.curSO.attackType)
        {
            case CardStats.AttackType.Melee:
                MeleeAttack();
                break;
            case CardStats.AttackType.Range:
                RangeAttack(true);
                break;
            case CardStats.AttackType.ShotGun:
                RangeAttack(false);
                break;
            case CardStats.AttackType.Magic:
                MagicAttack();
                break;
        }
        
        
       
        Move();
        Gambling();

        if(curCoolTime> 0)
        {
            attackCoolImage.fillAmount = curCoolTime / coolTime;
            curCoolTime-= Time.deltaTime;
        }

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

        float moveX = x * speed * Time.deltaTime;
        float moveY = y * speed * Time.deltaTime;
        transform.Translate(new Vector3(moveX,moveY,0),Space.World);
        
        //Rotation
        Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        _dir = (mousePosition - transform.position).normalized;
        _angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, _dir.x < 0 ? _angle + 180 : _angle);
        handle.transform.localScale = new Vector3(_dir.x<0?-1:1, 1);
     
       
    }

    private void MeleeAttack()
    { 
        if (Input.GetMouseButtonDown(0)&& curCoolTime <= 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.position + _dir, 1.5f);
            curCoolTime = coolTime;
            attackCoolImage.fillAmount = 1;
            animator.SetTrigger("SwordAttack");
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void RangeAttack(bool isRevolver)
    {
        if (Input.GetMouseButtonDown(0) && curCoolTime <= 0)
        {          
            curCoolTime = coolTime;
            attackCoolImage.fillAmount = 1;
            animator.SetTrigger("GunAttack");
            if (isRevolver)
                Attack.Inst.shootRevolver(_dir,transform,shootPoint,TypeManager.Inst.curSO.bulletCount,true);
            else
               Attack.Inst.shootShotgun(_dir, transform, shootPoint, TypeManager.Inst.curSO.bulletCount,true);             
        }           
    }

    private void MagicAttack()
    {
        if (Input.GetMouseButtonDown(0) && curCoolTime <= 0)
        {
            curCoolTime = coolTime;
            attackCoolImage.fillAmount = 1;
            animator.SetTrigger("SwordAttack");
            Attack.Inst.QueenMagic(shootPoint, TypeManager.Inst.curSO.bulletCount, true,opponent);
        }
    }


    public void MeleeDamage()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, transform.right, 2f, enemyMask);
        if(hit.collider != null)
        {
            hit.transform.GetComponent<Health>().OnDamage(PlayerStats.Inst.damage);
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
