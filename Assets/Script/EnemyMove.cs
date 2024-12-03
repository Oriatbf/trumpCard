
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMove : Character
{
    //나중에 캐릭터들 역할 수정 필요
    public enum EnemyCharacter { defaultEnemy,Boss}
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private CardStats card;
    [SerializeField] private float Gunrange,dashRange;
    [SerializeField] private bool haveDash;
    [SerializeField] private float midDistanceMoveCool;
    [SerializeField] private Image crown;
  
    private Vector2 _dir2,finalDir;
    public EnemyCharacter enemyCharacter;

    bool randomDir;

    private Camera _camera;

    public override void Start()
    {
        base.Start();
        if (GameManager.Inst.bossStage)
        {
            enemyCharacter = EnemyCharacter.Boss;
            crown.enabled= true;
        }
        else crown.enabled= false;
        opponent = GameObject.FindGameObjectWithTag("Player").transform;
        _dir = (opponent.position - transform.position).normalized;
        _camera = Camera.main;
       
      
      //  TypeManager.Inst.TypeChange(card.infor.cardNum, transform, true, characterSO);
        SetStat();

       
    }

    // Update is called once per frame
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3)) health.GetDamage(1000);
  
            base.Update();
            Rotation();
            Move();
           // CoolTime(characterSO);
        
           
    }
   
    
    private void Rotation()
    {
        //회전
        _angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
        handle.transform.parent.rotation = Quaternion.Euler(0, 0, _dir.x < 0 ? _angle + 180 : _angle);
        handle.transform.localScale = new Vector3(_dir.x < 0 ? -1 : 1, 1);
        _dir = (opponent.position - transform.position).normalized;

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
        float moveX = finalDir.x * stat.speed * Time.deltaTime;
        float moveY = finalDir.y * stat.speed * Time.deltaTime;
       
        transform.Translate(new Vector3(moveX,moveY,0),Space.World);

        if (haveDash)
        {
            /*
            Collider2D projectTileCol = Physics2D.OverlapCircle(transform.position, dashRange);
            if (projectTileCol.transform.CompareTag("ProjectTile"))
            {
                if (projectTileCol.GetComponentInParent<Projectile>().ownerCharacter != characterType)
                {
                    Vector2 dir = transform.position - projectTileCol.transform.position;
                    if (curCharging > 0 && curDashCool <= 0)
                    {
                        DashMove(dir.normalized);
                    }
                }
              
                
            }
            */
        }

    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,Gunrange);
        Gizmos.DrawWireSphere(transform.position, Gunrange-3);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, dashRange);
    }



}
