using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlooringCol : MonoBehaviour
{
    LineRenderer lineRenderer;
    PolygonCollider2D polygonCollider;

    public Transform FloorObj;
    public float pointSpacing = 0.1f; // 두 점 사이의 최소 거리
    public float lineThickness = 0.1f; // 라인의 두께
    private List<Vector2> points;
    private List<float> pointTimes;
    private Vector3 lastPoint;

    void Start()
    {
        lineRenderer= FloorObj.GetComponent<LineRenderer>();
        polygonCollider = FloorObj.GetComponent<PolygonCollider2D>();
        points = new List<Vector2>();
        pointTimes = new List<float>();
        ResetLineAndCol();
       

        AddPoint(transform.position); // 시작 위치 추가
    }

    void Update()
    {
        // 오브젝트가 이동하면 일정 거리 이상 이동한 경우에만 새로운 포인트 추가
        if (Vector3.Distance(transform.position, lastPoint) > pointSpacing)
        {
            AddPoint(transform.position);
        }

        RemoveOldPoints();
    }
    void RemoveOldPoints()
    {
        float currentTime = Time.time;

        // 1초가 지난 점들을 찾아 제거
        while (pointTimes.Count > 0 && currentTime - pointTimes[0] > 1.0f)
        {
            points.RemoveAt(0);       // 가장 오래된 점 제거
            pointTimes.RemoveAt(0);   // 해당 시간도 제거
        }

        // 라인렌더러와 콜라이더를 업데이트
        lineRenderer.positionCount = points.Count;
        if (points.Count > 0)
        {
            lineRenderer.SetPositions(points.ConvertAll(p => (Vector3)p).ToArray());
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
        UpdateCollider();
    }



    void AddPoint(Vector3 point)
    {
        // 새로운 점을 리스트에 추가하고 LineRenderer에 적용
        Vector2 point2D = new Vector2(point.x, point.y);
        points.Add(point2D);
        pointTimes.Add(Time.time);
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
        polygonCollider.pathCount = 0;
        if(points.Count > 0) points.Clear();
        pointTimes.Clear();

    }
}
