using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererTEst : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public PolygonCollider2D polygonCollider;
    Rigidbody2D rigid;
    public float pointSpacing = 0.1f; // 두 점 사이의 최소 거리
    public float lineThickness = 0.1f; // 라인의 두께
    private List<Vector2> points;
    private Vector3 lastPoint;

    void Start()
    {
        // LineRenderer와 PolygonCollider2D 초기화
        rigid= GetComponent<Rigidbody2D>();
        points = new List<Vector2>();

        AddPoint(transform.position); // 시작 위치 추가
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            ResetLineAndCol();
        }
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector2 dir = Camera.main.ScreenToWorldPoint(mousePosition) - transform.position;
            rigid.AddForce(dir.normalized*3f,ForceMode2D.Impulse);
        }
        // 오브젝트가 이동하면 일정 거리 이상 이동한 경우에만 새로운 포인트 추가
        if (Vector3.Distance(transform.position, lastPoint) > pointSpacing)
        {
            AddPoint(transform.position);
        }
    }

    void AddPoint(Vector3 point)
    {
        // 새로운 점을 리스트에 추가하고 LineRenderer에 적용
        Vector2 point2D = new Vector2(point.x, point.y);
        points.Add(point2D);
        lastPoint = point;

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ConvertAll(p => (Vector3)p).ToArray());

        UpdateCollider();
    }

    void UpdateCollider() //ㅈㄴ 어렵네
    {
        // 폴리곤 콜라이더의 경로를 업데이트
        List<Vector2> colliderPoints = new List<Vector2>();

        for (int i = 0; i < points.Count; i++)
        {
            Vector2 forward = Vector2.zero;
            if (i < points.Count - 1)
            {
                forward += (points[i + 1] - points[i]).normalized;
            }
            if (i > 0)
            {
                forward += (points[i] - points[i - 1]).normalized;
            }
            forward.Normalize();

            Vector2 normal = new Vector2(-forward.y, forward.x);

            colliderPoints.Add(points[i] + normal * lineThickness / 2);
            colliderPoints.Insert(0, points[i] - normal * lineThickness / 2);
        }

        polygonCollider.SetPath(0, colliderPoints.ToArray());
    }

    private void ResetLineAndCol()
    {
        lineRenderer.positionCount = 0;
        polygonCollider.pathCount= 0;
        points.Clear();
    }

}
