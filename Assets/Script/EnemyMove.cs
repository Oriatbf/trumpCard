
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
    [SerializeField] private float pathfindingInterval = 0.5f;
    private float _lastPathfindingTime;
    

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

        _lastPathfindingTime = Time.time;
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
        if (PathfindInterval() || pathfind.targetPos == Vector2Int.RoundToInt(transform.position))
        {
            if (IsBlocked(Vector2Int.RoundToInt(transform.position)))
            {
                state = State.Escape;
                Debug.Log("뒤가 막혔습니다. 탈출 경로를 찾습니다.");
            }
            else if (_distance.sqrMagnitude < Gunrange * Gunrange && _distance.sqrMagnitude > (Gunrange - 3) * (Gunrange - 3))
            {
                Debug.Log("플레이어와 중간거리");
                switch (stat.cardType)
                {
                    case CardType.Melee:
                        state = State.CloseToOpponent;
                        break;
                    case CardType.Range:
                        state = State.Random;
                        break;
                }
               
            }
            else if (_distance.sqrMagnitude < (Gunrange - 3) * (Gunrange - 3))
            {
                Debug.Log("플레이어와 가까움");
                switch (stat.cardType)
                {
                    case CardType.Melee:
                        state = State.Inplace;
                        break;
                    case CardType.Range:
                        state = State.FarToOpponent;
                        break;
                }
                
            }
            else if (_distance.sqrMagnitude > Gunrange * Gunrange)
            {
                Debug.Log("플레이어와 멀리 떨어짐");
                switch (stat.cardType)
                {
                    case CardType.Melee:
                        state = State.CloseToOpponent;
                        break;
                    case CardType.Range:
                        state = State.CloseToOpponent;
                        break;
                }
            }
            else
            {
                Debug.Log("아무상태도아님");
            }
        }
        

        switch (state)
        {
            case State.CloseToOpponent:
                PathfindClose();
                break;
            case State.FarToOpponent:
                PathfindFar();
                break;
            case State.Random:
                PathfindRandom();
                break;
            case State.Escape:
                PathfindEscape();
                break;
            case State.Inplace:
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
        PathfindMove();

        Vector2Int newTargetPos = Vector2Int.RoundToInt(GameManager.Inst.GetOpponent(this).transform.position);
        
        // 동일한 목표 위치라면 경로 탐색 생략
        if (pathfind.targetPos == newTargetPos && currentPathIndex < pathToPlayer.Count)
            return;

        pathfind.startPos = Vector2Int.RoundToInt(transform.position);
        pathfind.targetPos = newTargetPos;
        pathfind.PathFinding();
        pathToPlayer = pathfind.FinalNodeList;
        currentPathIndex = 0; // 새로운 경로일 때만 초기화
        
       
      
    }

    private bool PathfindInterval()
    {
        if (Time.time - _lastPathfindingTime >= pathfindingInterval) //쿨타임이 지났다면
        {
            _lastPathfindingTime = Time.time;
            Debug.Log("쿨타임끝");
            return true;
        }
        else return false;
    }

    private void PathfindFar()
    {
        PathfindMove();

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
            pathfind.PathFinding();
            pathToPlayer = pathfind.FinalNodeList;
            currentPathIndex = 0; 
        }
    }
    
    private void PathfindEscape()
    {
        PathfindMove();

        Vector2Int opponentPosition = Vector2Int.RoundToInt(GameManager.Inst.GetOpponent(this).transform.position);
        Vector2Int currentPosition = Vector2Int.RoundToInt(transform.position);
        
        Vector2Int targetPosition  = Vector2Int.one;
        
        targetPosition = FindEscapeTarget(currentPosition, opponentPosition);
        
        if (pathfind.targetPos != targetPosition)
        {
            pathfind.startPos = currentPosition;
            pathfind.targetPos = targetPosition;
            pathfind.PathFinding();

            pathToPlayer = pathfind.FinalNodeList;
            currentPathIndex = 0;
        }
    }
    
    private void PathfindRandom()
    {
        PathfindMove();

        Vector2Int opponentPosition = Vector2Int.RoundToInt(GameManager.Inst.GetOpponent(this).transform.position);
        Vector2Int currentPosition = Vector2Int.RoundToInt(transform.position);
        
        Vector2Int targetPosition  = Vector2Int.one;
        
        targetPosition = PathfindRandomPos(currentPosition, opponentPosition);
        
        //이부분 문제있음
        if (pathfind.targetPos == currentPosition)
        {
            pathfind.startPos = currentPosition;
            pathfind.targetPos = targetPosition;
            pathfind.PathFinding();

            pathToPlayer = pathfind.FinalNodeList;
            currentPathIndex = 0;
        }
    }

    private void PathfindMove()
    {
        // 경로가 있을 경우 따라감
        if (pathToPlayer.Count > 0 && currentPathIndex < pathToPlayer.Count)
        {
            Node targetNode = pathToPlayer[currentPathIndex];
            Vector2 targetPosition = new Vector2(targetNode.x, targetNode.y);
            Vector2 currentPosition = transform.position;

            // 목표 위치로 이동
            transform.position = Vector2.MoveTowards(currentPosition, targetPosition, 4 * Time.deltaTime);
            // 목표 위치에 도달했으면 다음 경로로 이동
            if ((targetPosition - currentPosition).sqrMagnitude < 0.01f)
            {
                currentPathIndex++;
            }
        }
    }


    
    // 탈출 경로 계산
    private Vector2Int FindEscapeTarget(Vector2Int currentPosition, Vector2Int opponentPosition)
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
                int distance = Mathf.Abs(newPosition.x - opponentPosition.x) + Mathf.Abs(newPosition.y - opponentPosition.y);

                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    bestPosition = newPosition;
                }
            }
        }

        return bestPosition;
    }

    private Vector2Int PathfindRandomPos(Vector2Int currentPosition, Vector2Int opponentPosition)
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

        List<Vector2Int> validDirections = new List<Vector2Int>();

        foreach (Vector2Int direction in possibleDirections)
        {
            Vector2Int newPosition = currentPosition + direction;
            if (!IsBlocked(newPosition))
            {
                validDirections.Add(newPosition);
            }
        }

        // 유효한 방향이 있을 때만 랜덤 선택
        if (validDirections.Count > 0)
        {
            int randomIndex = Random.Range(0, validDirections.Count);
            return validDirections[randomIndex];
        }
        else
        {
            Debug.Log("자신의 위치를 반환");
            return currentPosition;  // 블록된 경우 현재 위치 반환
        }
        
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
