using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PathfindAI : MonoBehaviour
{
    [Serializable]
    public class CoolTime
    {
        public float startTime;
        public float interval;
        public Action action;

        public CoolTime(float _interval, Action _action)
        {
            startTime = Time.time;
            interval = _interval;
            action = _action;
        }
    }

    [SerializeField] private List<CoolTime> coolTimeList = new List<CoolTime>();
    
    private PathFind pathfind;
    private int currentPathIndex = 0;
    private List<Node> pathToPlayer = new List<Node>();
    private Vector2Int opponentPos,currentPos;
    private CharacterType characterType;
    private float pathfindingInterval;
    private float _lastPathfindingTime;
    private float speed;
    public BattleState battleState;
    public Vector2Int randomPos;

    private void Awake()
    {
        pathfind = GetComponent<PathFind>();
    }

    public void Init(CharacterType _characterType,float _speed)
    {
        characterType = _characterType;
        speed = _speed;
    }

    private void Update()
    {
        if(GameManager.Inst.Pause()) return;
        randomPos =  FindRandomTarget(false);
        opponentPos = Vector2Int.RoundToInt(GameManager.Inst.GetOpponent(characterType).transform.position);
        currentPos = Vector2Int.RoundToInt(transform.position);
        if (coolTimeList.Count > 0)
        {
            foreach (var coolTime in coolTimeList)
            {
                if (Time.time - coolTime.startTime >= coolTime.interval)
                {
                    coolTime.startTime = Time.time;
                    coolTime.action?.Invoke();
                    Debug.Log("행동");
                }
            }
        }
        StateChange();
    }

    public void StateChange()
    {
        switch (battleState)
        {
            case BattleState.CloseToOpponent:
                PathfindClose();
                break;
            case BattleState.FarToOpponent:
                PathfindFar();
                break;
            case BattleState.Random:
                PathfindRandom();
                break;
            case BattleState.Escape:
                PathfindEscape();
                break;
            case BattleState.Inplace:
                break;
        }
    }

    public void AddCoolTime(float _interval,Action _action)
    {
        coolTimeList.Add(new CoolTime(_interval,_action));
    }
    
    public bool PathfindInterval()
    {
        if (Time.time - _lastPathfindingTime >= pathfindingInterval) //쿨타임이 지났다면
        {
            _lastPathfindingTime = Time.time;
            Debug.Log("쿨타임끝");
            return true;
        }
        else return false;
    }

    
    public void PathfindClose()
    {
        PathfindMove();

        Vector2Int newTargetPos = Vector2Int.RoundToInt(GameManager.Inst.GetOpponent(characterType).transform.position);
        
        // 동일한 목표 위치라면 경로 탐색 생략
        if (pathfind.targetPos == newTargetPos && currentPathIndex < pathToPlayer.Count)
            return;

        pathfind.startPos = Vector2Int.RoundToInt(transform.position);
        pathfind.targetPos = newTargetPos;
        pathfind.PathFinding();
        pathToPlayer = pathfind.FinalNodeList;
        currentPathIndex = 0; // 새로운 경로일 때만 초기화
        
       
      
    }


    public void PathfindFar()
    {
        PathfindMove();

        Vector2 opponentPos = this.opponentPos;
        Vector2 curPos = currentPos;

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
    
    public void PathfindEscape()
    {
        PathfindMove();

        Vector2Int opponentPosition = opponentPos;
        Vector2Int currentPosition = currentPos;
        
        Vector2Int targetPosition  = Vector2Int.one;
        
        targetPosition = FindRandomTarget(true);
        
        if (pathfind.targetPos != targetPosition)
        {
            pathfind.startPos = currentPosition;
            pathfind.targetPos = targetPosition;
            pathfind.PathFinding();

            pathToPlayer = pathfind.FinalNodeList;
            currentPathIndex = 0;
        }
    }
    
    public void PathfindRandom()
    {
        PathfindMove();
        
        if (pathfind.targetPos != randomPos)
        {
            pathfind.startPos = currentPos;
            pathfind.targetPos = randomPos;
            pathfind.PathFinding();

            pathToPlayer = pathfind.FinalNodeList;
            currentPathIndex = 0;
        }
    }
    
    
    // 탈출 경로 계산
    private Vector2Int FindRandomTarget(bool escape)
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
        
        foreach (var dir in possibleDirections)
        {
            Vector2Int newPosition = currentPos + dir;
            if (!IsBlocked(newPosition))
                validDirections.Add(newPosition);
        }
        
        Vector2Int bestPosition = currentPos;
        if (escape)
        {
            #region 탈출로 찾기
            int maxDistance = 0;
            foreach (Vector2Int newPosition in validDirections)
            {
                int distance = Mathf.Abs(newPosition.x - opponentPos.x) + Mathf.Abs(newPosition.y - opponentPos.y);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    bestPosition = newPosition;
                }
            }
            #endregion
        }
        else
        {
            #region 랜덤 방향 찾기
            // 유효한 방향이 있을 때만 랜덤 선택
            if (validDirections.Count > 0)
            {
                int randomIndex = Random.Range(0, validDirections.Count);
                bestPosition = validDirections[randomIndex];
            }
            #endregion
        }
        return bestPosition;
    }
    
    public bool IsBlocked(Vector2Int position) //근처에 벽이 있는지 감지
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

    private void PathfindMove()
    {
        // 경로가 있을 경우 따라감
        if (pathToPlayer.Count > 0 && currentPathIndex < pathToPlayer.Count)
        {
            Node targetNode = pathToPlayer[currentPathIndex];
            Vector2 targetPosition = new Vector2(targetNode.x, targetNode.y);
            Vector2 currentPosition = transform.position;

            // 목표 위치로 이동
            transform.position = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);
            // 목표 위치에 도달했으면 다음 경로로 이동
            if ((targetPosition - currentPosition).sqrMagnitude < 0.01f)
            {
                currentPathIndex++;
            }
        }
    }

}
