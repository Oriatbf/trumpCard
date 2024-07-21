using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEst : MonoBehaviour
{


    public Transform target;
    public float setRad, getRad;
    public float speed;
    private float t;
    Vector2 pos;

    private RandomBezier randomBezier;
    // Start is called before the first frame update
    void Start()
    {
        randomBezier = new RandomBezier(transform.position,target.position,setRad,getRad);
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Bezier.GetPoint(randomBezier, t);
        t += Time.deltaTime * speed;

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = pos;
            t = 0;
        }
    }
}
