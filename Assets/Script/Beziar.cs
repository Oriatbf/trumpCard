using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct RandomBezier
{
    public Vector2 p0, p1, p2, p3;

    public RandomBezier(Vector2 setter, Vector2 getter,float setRadius,float getRadius)
    {
        p0 = setter;
        p3 = getter;

        p1 = setter + Random.insideUnitCircle * setRadius;
        p2 = getter+ Random.insideUnitCircle * getRadius;
    }

}

public static class Bezier
{
   

    public static Vector2  GetPoint(Vector2 p0, Vector2 p1,Vector2 p2, Vector2 p3,float t)
    {
        t = Mathf.Clamp01(t);
        var oneMinusT = 1 - t;

        return Mathf.Pow(oneMinusT, 3) * p0 +
               3 * Mathf.Pow(oneMinusT, 2) * t * p1 +
               3 * oneMinusT * Mathf.Pow(t, 2) * p2 +
               Mathf.Pow(t, 3) * p3;
    }

    public static Vector2 GetPoint(RandomBezier randomBezier,  float t)
    {
       return GetPoint(randomBezier.p0, randomBezier.p1, randomBezier.p2, randomBezier.p3,t);
    }
}
