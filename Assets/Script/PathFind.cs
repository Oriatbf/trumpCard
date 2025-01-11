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
    public int x, y, G, H;
    public int F
    {
        get {return G + H; }
    }

    public Node(bool _isWall, int _x, int _y)
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

    private int sizeX, sizeY;
    private Node[,] NodeArray;//타일맵의 판의 크기 (이차원 배열이라는 뜻)
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
        sizeX = topRight.x - bottomLeft.x + 1;
        sizeY = topRight.y - bottomLeft.y + 1;
        NodeArray = new Node[sizeX, sizeY];
        
        //벽 찾기
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                bool isWall = false;
                foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i + bottomLeft.x, j + bottomLeft.y), 0.4f))
                    if (col.gameObject.layer == LayerMask.NameToLayer("Wall")) isWall = true;

                NodeArray[i, j] = new Node(isWall, i + bottomLeft.x, j + bottomLeft.y);
            }
        }
        #endregion
    }

    [Button]
    public void PathFinding()
    {
        targetPos = new Vector2Int(Mathf.Clamp(targetPos.x, bottomLeft.x, topRight.x), 
            Mathf.Clamp(targetPos.y, bottomLeft.y, topRight.y));
        // 시작과 끝 노드, 열린리스트와 닫힌리스트, 마지막리스트 초기화
        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y]; //NodeArray의 배열 인덱스는 0부터 시작하기 때문에 bottomLeft를 뺴줌
        TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];

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
                OpenListAdd(CurNode.x + 1, CurNode.y + 1);
                OpenListAdd(CurNode.x - 1, CurNode.y + 1);
                OpenListAdd(CurNode.x - 1, CurNode.y - 1);
                OpenListAdd(CurNode.x + 1, CurNode.y - 1);
            }

            // ↑ → ↓ ←
            OpenListAdd(CurNode.x, CurNode.y + 1);
            OpenListAdd(CurNode.x + 1, CurNode.y);
            OpenListAdd(CurNode.x, CurNode.y - 1);
            OpenListAdd(CurNode.x - 1, CurNode.y);
            
        }
    }

    private void OpenListAdd(int checkX,int checkY)
    {
        if (checkX >= bottomLeft.x && checkX <= topRight.x && checkY >= bottomLeft.y && checkY <= topRight.y  //범위 안에 있눈지
            && !NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y].isWall //벽인지
            && !ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y])) //CLosedList에 없는지
        {
            // 대각선 허용시, 벽 사이로 통과 안됨
            if (allowDiagonal) if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall && NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return; //벽 두개 사이로 통과 불가능

            // 코너를 가로질러 가지 않을시, 이동 중에 수직수평 장애물이 있으면 안됨
            if (dontCrossCorner) if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall || NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall) return; //벽 하나라도 있으면 통과 불가느

            // 이웃노드에 넣고, 직선은 10, 대각선은 14비용
            Node NeighborNode = NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14);

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
                Gizmos.DrawSphere(new Vector3(n.x,n.y),0.1f);
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
