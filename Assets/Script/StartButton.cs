using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public Vector2 size,pos;
    public LayerMask playerLayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.OverlapBox(transform.position + (Vector3)pos, size, 0, playerLayer))
        {
            Debug.Log("ssdadw");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + (Vector3)pos, size);
    }
}
