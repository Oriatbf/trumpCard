
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemyMove : Character
{

    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private CardStats card;
    [SerializeField] private float Gunrange,dashRange;
    [SerializeField] private bool haveDash;
    [SerializeField] private float midDistanceMoveCool;
  
    private float _angle;
    private Vector2 _dir2,finalDir;

    bool randomDir;

    private Camera _camera;

   
    
    public override void Start()
    {
        opponent = GameObject.FindGameObjectWithTag("Player").transform;
        _dir = (opponent.position - transform.position).normalized;
        _camera = Camera.main;
        TypeManager.Inst.TypeChange(card.infor.cardNum, transform, false,characterSO);

        SetStat();

        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (GameManager.Inst.isGameStart)
        {
            base.Update();
            Rotation();
            Move();
            CoolTime(characterSO);
            Gambling();
        }
           
    }
    private void Gambling()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TypeManager.Inst.TypeChange(GambleManager.GambleIndex(), transform, isPlayer,characterSO);
        }
    }

    private void Rotation()
    {
        //회전
        _angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
        handle.transform.parent.rotation = Quaternion.Euler(0, 0, _dir.x < 0 ? _angle + 180 : _angle);
        handle.transform.localScale = new Vector3(_dir.x < 0 ? -1 : 1, 1);
        _dir = (opponent.position - transform.position);

        if (!isDashing)
        {
            if (_dir.sqrMagnitude > Gunrange * Gunrange && !randomDir)
            {

                finalDir = _dir.normalized;

            }

            if (_dir.sqrMagnitude < Gunrange * Gunrange && _dir.sqrMagnitude > (Gunrange - 3) * (Gunrange - 3) && !randomDir)
            {

                finalDir = Random.insideUnitCircle.normalized;
                randomDir = true;
                DOVirtual.DelayedCall(midDistanceMoveCool, () => randomDir = false);
            }


            if (_dir.sqrMagnitude < (Gunrange - 3) * (Gunrange - 3) && !randomDir)
            {
                _dir2 = (transform.position - opponent.position).normalized;
                finalDir = _dir2;
            }
        }
      
    }
   


    private void Move()
    {
        finalDir =  finalDir.normalized;
        //이동
        float moveX = finalDir.x * speed * Time.deltaTime;
        float moveY = finalDir.y * speed * Time.deltaTime;
       
        transform.Translate(new Vector3(moveX,moveY,0),Space.World);

        if (haveDash)
        {
            Collider2D projectTileCol = Physics2D.OverlapCircle(transform.position, dashRange);
            if (projectTileCol.transform.CompareTag("ProjectTile"))
            {
                if (projectTileCol.GetComponentInParent<Projectile>().isPlayerBullet)
                {
                    Vector2 dir = transform.position - projectTileCol.transform.position;
                    if (curCharging > 0 && curDashCool <= 0)
                    {
                        DashMove(dir.normalized);
                    }
                }
              
                
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,Gunrange);
        Gizmos.DrawWireSphere(transform.position, Gunrange-3);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, dashRange);
    }



}
