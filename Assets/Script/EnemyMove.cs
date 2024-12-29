
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EnemyMove : Character
{
    //나중에 캐릭터들 역할 수정 필요
    public enum EnemyCharacter { defaultEnemy,Boss}

    public enum AttackType
    {
        Melee,Range
    }

    public enum State
    {
        CloseToOpponent,Inplace,FarToOpponent,Random
    }

    [SerializeField] private float Gunrange;
    [SerializeField] private float bulletDetectRange;
    [SerializeField] private bool haveDash;
    [SerializeField] private float midDistanceMoveCool;
    [SerializeField] private Image crown;
  
    private Vector2 _dir2,finalDir;
    public EnemyCharacter enemyCharacter;
    public State state;

    bool moveStop = false;
    private float angleInRadians;

    public override void Start()
    {
        if (GameManager.Inst.bossStage)
        {
            enemyCharacter = EnemyCharacter.Boss;
            crown.enabled= true;
        }
        else crown.enabled= false;
        opponent = GameManager.Inst.GetOpponent(this);
        _dir = (opponent.transform.position - transform.position).normalized;
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3)) health.GetDamage(1000);
        _dir = (opponent.transform.position - transform.position).normalized;
  
        base.Update();
        Rotation();
        StateChange();
        
    }

    public void StateChange()
    {
        Vector3 _distance = (opponent.transform.position - transform.position);
        
        if (_distance.sqrMagnitude > Gunrange * Gunrange )
        {
            state = State.CloseToOpponent;
        }
        
        if (_distance.sqrMagnitude < (Gunrange - 3) * (Gunrange - 3) )
        {
            state = State.FarToOpponent;
        }
        
        if (_distance.sqrMagnitude < Gunrange * Gunrange && _distance.sqrMagnitude > (Gunrange - 3) * (Gunrange - 3))
        {
            state = State.Random;
        }

        switch (state)
        {
            case State.CloseToOpponent:
                CloseToPlayer();

                break;
            case State.FarToOpponent:
                FarToPlayer();
                

                break;
            case State.Random:
                RandomMove();
                break;
        }
        
    }
   
    
    private void Rotation()
    {
        //회전
        _angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
        handle.transform.parent.rotation = Quaternion.Euler(0, 0, _dir.x < 0 ? _angle + 180 : _angle);
        handle.transform.localScale = new Vector3(_dir.x < 0 ? -1 : 1, 1);
    }

    private void CircleMove()
    {
     
        // 현재 객체와 opponent 사이의 거리 계산 (반지름)
        Vector2 offset = transform.position - opponent.transform.position;
        float radius = offset.magnitude;
        float angleInDegress = Mathf.Atan2(transform.position.y, transform.position.x) * Mathf.Rad2Deg;
        angleInDegress += 10 * Time.deltaTime;
        


        // 각도 업데이트 (speed에 따라 변화)
        angleInRadians =angleInDegress*Mathf.Deg2Rad;

        // 새로운 x, y 좌표 계산 (원의 중심 opponent 기준)
        float newX = opponent.transform.position.x + Mathf.Cos(angleInRadians) * radius;
        float newY = opponent.transform.position.y + Mathf.Sin(angleInRadians) * radius;

        // 객체 위치 업데이트
        transform.position = new Vector3(newX, newY, transform.position.z);

    }

    public void CloseToPlayer()
    {
        finalDir = _dir.normalized;
        Debug.Log("플레이어한테 이동");
        Move(finalDir);
    }

    public void RandomMove()
    {
        if (!moveStop)
        {
            moveStop = true;
            finalDir = Random.insideUnitCircle.normalized;
            DOVirtual.DelayedCall(1f, () => moveStop = false);
        }
        
        Debug.Log("랜덤 이동");
        Move(finalDir,1/stat.originStatValue.speed);
    }

    public void FarToPlayer()
    {
        _dir2 = (transform.position - opponent.transform.position).normalized;
        Debug.Log("플레이어한테 멀어지기");
        finalDir = _dir2;
        Move(finalDir);
    }
   


    private void Move(Vector2 finalDir,float time = 0)
    {
        
        finalDir =  finalDir.normalized;
        //이동
        float moveX = finalDir.x * stat.originStatValue.speed * Time.deltaTime;
        float moveY = finalDir.y * stat.originStatValue.speed * Time.deltaTime;
       
        transform.Translate(new Vector3(moveX,moveY,0),Space.World);
            
        Collider2D[] projectTileCol = Physics2D.OverlapCircleAll(transform.position, bulletDetectRange);
        Bullet approachBullet = null; //근접한 총알
        foreach (Collider2D col in projectTileCol)
        {
            if(!col.transform.parent)return;
            if (col.transform.parent.TryGetComponent(out Bullet bullet) && bullet.ownerCharacter != characterType)
            {
                approachBullet = bullet;
                break;
            }
        }

        if (approachBullet)
        {
            Debug.Log("피하기");
            Vector2 dir = transform.position - approachBullet.transform.position;
           // DashMove(dir.normalized);
 
        }

    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,Gunrange);
        Gizmos.DrawWireSphere(transform.position, Gunrange-3);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, bulletDetectRange);
    }



}
