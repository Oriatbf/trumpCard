using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VInspector;

[Serializable]
public class Node
{
    public bool isWall;
    public Node parentNode;
    public float x, y, G, H;
    public float F
    {
        get {return G + H; }
    }

    public Node(bool _isWall,float _x, float _y)
    {
        isWall = _isWall;
        x = _x;
        y = _y;
    }
}
public class PathFind : MonoBehaviour
{
    public Vector2Int bottomLeft, topRight,startPos,targetPos;
    public List<Node> FinalNodeList = new List<Node>(); //최종 도착지점까지의 노드
    [SerializeField] private bool allowDiagonal, dontCrossCorner;

    private float NodeIntervalSize = 0.5f;

    private int sizeX, sizeY;
    private Node[,] NodeArray;//타일맵의 판의 크기 (이차원 배열이라는 뜻) 
 //   private Dictionary<Vector2, Node> NodeArray = new Dictionary<Vector2, Node>();
    private Node StartNode, TargetNode, CurNode;

    private List<Node> OpenList = new List<Node>(); //아직 뻗지 않은 노드를 저장
    private List<Node> ClosedList = new List<Node>(); //이미 간 노드를 저장

    private void Awake()
    {
        startPos = Vector2Int.RoundToInt(transform.position);
        NodeSetting();
    }

    private void NodeSetting()
    {
        #region 처음 세팅
        sizeX = Mathf.RoundToInt((topRight.x - bottomLeft.x) / NodeIntervalSize) + 1;
        sizeY = Mathf.RoundToInt((topRight.y - bottomLeft.y) / NodeIntervalSize) + 1;
        NodeArray = new Node[sizeX, sizeY];
        
        //벽 찾기
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                Vector2 nodePosition =
                    new Vector2(i * NodeIntervalSize + bottomLeft.x, j * NodeIntervalSize + bottomLeft.y);
                bool isWall = false;
                foreach (Collider2D col in Physics2D.OverlapCircleAll(nodePosition, 0.4f))
                    if (col.gameObject.layer == LayerMask.NameToLayer("Wall")) isWall = true;

                NodeArray[i,j] = new Node(isWall, nodePosition.x, nodePosition.y);
            }
        }
        #endregion
    }

    int IntervalInt(float n)
    {
        return Mathf.RoundToInt(n / NodeIntervalSize);
    }

    [Button]
    public void PathFinding()
    {
        targetPos = new Vector2Int(Mathf.Clamp(targetPos.x, bottomLeft.x, topRight.x), 
            Mathf.Clamp(targetPos.y, bottomLeft.y, topRight.y));
        // 시작과 끝 노드, 열린리스트와 닫힌리스트, 마지막리스트 초기화
        StartNode = NodeArray[IntervalInt(startPos.x - bottomLeft.x), IntervalInt(startPos.y - bottomLeft.y)]; //NodeArray의 배열 인덱스는 0부터 시작하기 때문에 bottomLeft를 뺴줌
        TargetNode = NodeArray[IntervalInt(targetPos.x - bottomLeft.x), IntervalInt(targetPos.y - bottomLeft.y)];

        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();

        while (OpenList.Count > 0)
        {
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);
            
            //마지막
            if (CurNode == TargetNode)
            {
                Node TargetCurNode = TargetNode;
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.parentNode;
                }
                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();
                
                //for (int i = 0; i < FinalNodeList.Count; i++) print(i + "번째는 " + FinalNodeList[i].x + ", " + FinalNodeList[i].y);
                return;
            }
            
            // ↗ ↖ ↙ ↘
            if (allowDiagonal)
            {
                OpenListAdd(CurNode.x + NodeIntervalSize, CurNode.y + NodeIntervalSize);
                OpenListAdd(CurNode.x - NodeIntervalSize, CurNode.y + NodeIntervalSize);
                OpenListAdd(CurNode.x - NodeIntervalSize, CurNode.y - NodeIntervalSize);
                OpenListAdd(CurNode.x + NodeIntervalSize, CurNode.y - NodeIntervalSize);
            }

            // ↑ → ↓ ←
            OpenListAdd(CurNode.x, CurNode.y + NodeIntervalSize);
            OpenListAdd(CurNode.x + NodeIntervalSize, CurNode.y);
            OpenListAdd(CurNode.x, CurNode.y - NodeIntervalSize);
            OpenListAdd(CurNode.x - NodeIntervalSize, CurNode.y);
            
        }
    }

    private void OpenListAdd(float checkX,float checkY)
    {
        if (checkX >= bottomLeft.x && checkX <= topRight.x && checkY >= bottomLeft.y && checkY <= topRight.y  //범위 안에 있눈지
            && !NodeArray[IntervalInt(checkX - bottomLeft.x), IntervalInt(checkY - bottomLeft.y)].isWall //벽인지
            && !ClosedList.Contains(NodeArray[IntervalInt(checkX - bottomLeft.x),IntervalInt( checkY - bottomLeft.y)])) //CLosedList에 없는지
        {
            // 대각선 허용시, 벽 사이로 통과 안됨
            if (allowDiagonal) if (NodeArray[IntervalInt(CurNode.x - bottomLeft.x),IntervalInt(checkY - bottomLeft.y)].isWall 
                                   && NodeArray[IntervalInt(checkX - bottomLeft.x),IntervalInt(CurNode.y - bottomLeft.y)].isWall) return; //벽 두개 사이로 통과 불가능

            // 코너를 가로질러 가지 않을시, 이동 중에 수직수평 장애물이 있으면 안됨
            if (dontCrossCorner) if (NodeArray[IntervalInt(CurNode.x - bottomLeft.x),IntervalInt(checkY - bottomLeft.y)].isWall 
                                     || NodeArray[IntervalInt(checkX - bottomLeft.x),IntervalInt(CurNode.y - bottomLeft.y)].isWall) return; //벽 하나라도 있으면 통과 불가느

            // 이웃노드에 넣고, 직선은 10, 대각선은 14비용
            Node NeighborNode = NodeArray[IntervalInt(checkX - bottomLeft.x),IntervalInt(checkY - bottomLeft.y)];
            float MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14);

            if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.y - TargetNode.y)) * 10;
                NeighborNode.parentNode = CurNode;
                OpenList.Add(NeighborNode);
            }

        }
    }
    
    void OnDrawGizmos()
    {
      
        Gizmos.color = Color.red;
        if(ClosedList.Count > 0) 
        {
            foreach (var n in NodeArray)
            {
                Gizmos.DrawSphere(new Vector2(n.x,n.y),0.1f);
            }
        }
        
        Gizmos.color = Color.blue;
        if(FinalNodeList.Count != 0) 
        {
            for (int i = 0; i < FinalNodeList.Count - 1; i++)
                Gizmos.DrawLine(new Vector2(FinalNodeList[i].x, FinalNodeList[i].y), new Vector2(FinalNodeList[i + 1].x, FinalNodeList[i + 1].y));
        }
    }

}
