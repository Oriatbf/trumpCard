
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
        CloseToOpponent,Inplace,FarToOpponent,Random,Escape
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

    private PathFind pathfind;
    private List<Node> pathToPlayer = new List<Node>();
    private int currentPathIndex;

    public override void Awake()
    {
        pathfind = GetComponent<PathFind>();
        base.Awake();
    }

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
        if (Input.GetKeyDown(KeyCode.F3)) unitHealth.GetDamage(1000);
        _dir = (opponent.transform.position - transform.position).normalized;
  
        base.Update();
        Rotation();
        StateChange();
        
    }

    public void StateChange()
    {
        Vector3 _distance = (opponent.transform.position - transform.position);
        
        if (IsBlocked(Vector2Int.RoundToInt(transform.position)))
        {
            state = State.Escape;
            Debug.Log("뒤가 막혔습니다. 탈출 경로를 찾습니다.");
        }
        else if (_distance.sqrMagnitude < Gunrange * Gunrange && _distance.sqrMagnitude > (Gunrange - 3) * (Gunrange - 3))
        {
            state = State.Random;
        }
        else if (_distance.sqrMagnitude < (Gunrange - 3) * (Gunrange - 3))
        {
            state = State.FarToOpponent;
        }
        else if (_distance.sqrMagnitude > Gunrange * Gunrange)
        {
            state = State.CloseToOpponent;
        }

        switch (state)
        {
            case State.CloseToOpponent:
                PathfindFar();

                break;
            case State.FarToOpponent:
                PathfindFar();
                break;
            case State.Random:
                PathfindFar();
                Debug.Log("랜덤이동");
                //RandomMove();
                break;
            case State.Escape:
                PathfindEscape();
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


    private void PathfindClose()
    {
        Vector2Int newTargetPos = Vector2Int.RoundToInt(GameManager.Inst.GetOpponent(this).transform.position);
        if (pathfind.targetPos != newTargetPos)
        {
            pathfind.startPos = Vector2Int.RoundToInt(transform.position);
            pathfind.targetPos = newTargetPos;
            pathfind.PathFinding();
            pathToPlayer = pathfind.FinalNodeList;
            currentPathIndex = 0; // 새로운 경로일 때만 초기화
        }
        PathfindMove();
      
    }

    private void PathfindFar()
    {
        Vector2 opponentPos = GameManager.Inst.GetOpponent(this).transform.position;
        Vector2 curPos = transform.position;

        Vector2 direction = curPos - opponentPos;
        Vector2 newTargetPos = curPos + direction.normalized * 1f;
        Vector2Int newTargetPosInt = Vector2Int.RoundToInt(newTargetPos);
        newTargetPosInt = new Vector2Int(Mathf.Clamp(newTargetPosInt.x, pathfind.bottomLeft.x, pathfind.topRight.x), 
            Mathf.Clamp(newTargetPosInt.y, pathfind.bottomLeft.y, pathfind.topRight.y));
        if (pathfind.targetPos != newTargetPosInt)
        {
            pathfind.startPos = Vector2Int.RoundToInt(transform.position);
            pathfind.targetPos = newTargetPosInt;
            Debug.Log(newTargetPosInt);
            pathfind.PathFinding();
            pathToPlayer = pathfind.FinalNodeList;
            currentPathIndex = 0; 
        }
        PathfindMove();

        
    }
    
    private void PathfindEscape()
    {
        Vector2Int opponentPosition = Vector2Int.RoundToInt(GameManager.Inst.GetOpponent(this).transform.position);
        Vector2Int currentPosition = Vector2Int.RoundToInt(transform.position);
        
        Vector2Int targetPosition  = Vector2Int.one;

  
        targetPosition = FindEscapeTarget(currentPosition, opponentPosition);
        

        if (pathfind.targetPos != targetPosition)
        {
            pathfind.startPos = currentPosition;
            Debug.Log(targetPosition);
            pathfind.targetPos = targetPosition;
            pathfind.PathFinding();

            pathToPlayer = pathfind.FinalNodeList;
            currentPathIndex = 0;
        }
      
        
        PathfindMove();
    }

    private void PathfindMove()
    {
        // 경로가 있을 경우 따라감
        if (pathToPlayer.Count > 0 && currentPathIndex < pathToPlayer.Count)
        {
            Node targetNode = pathToPlayer[currentPathIndex];
            Vector2 targetPosition = new Vector2(targetNode.x, targetNode.y);
            Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);

            // 목표 위치로 이동
            transform.position = Vector2.MoveTowards(currentPosition, targetPosition, 4 * Time.deltaTime);
            // 목표 위치에 도달했으면 다음 경로로 이동
            if (Vector2.Distance(currentPosition, targetPosition) < 0.1f)
            {
                currentPathIndex++;
                
            }
        }
    }


    
    // 탈출 경로 계산
    private Vector2Int FindEscapeTarget(Vector2Int currentPosition, Vector2Int playerPosition)
    {
        List<Vector2Int> possibleDirections = new List<Vector2Int>
        {
            new Vector2Int(0, 1),  // 위
            new Vector2Int(1, 0),  // 오른쪽
            new Vector2Int(0, -1), // 아래
            new Vector2Int(-1, 0), // 왼쪽
            new Vector2Int(1, 1),  // ↗
            new Vector2Int(-1, 1), // ↖
            new Vector2Int(-1, -1),// ↙
            new Vector2Int(1, -1)  // ↘
        };

        Vector2Int bestPosition = currentPosition;
        int maxDistance = 0;

        foreach (Vector2Int direction in possibleDirections)
        {
            Vector2Int newPosition = currentPosition + direction;

            if (!IsBlocked(newPosition))
            {
                int distance = Mathf.Abs(newPosition.x - playerPosition.x) + Mathf.Abs(newPosition.y - playerPosition.y);

                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    bestPosition = newPosition;
                }
            }
        }

        return bestPosition;
    }
    
    // 이동 가능 여부 체크
    private bool IsBlocked(Vector2Int position)
    {
        foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(position.x, position.y), 1f))
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                return true;
            }
        }
        return false;
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,1);
    }



}
