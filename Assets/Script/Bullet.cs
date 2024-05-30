using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rigid;
    Vector2 bulletDir;
    // Start is called before the first frame update
    void Start()
    {

        rigid= GetComponent<Rigidbody2D>();
        rigid.AddForce(bulletDir * 5f, ForceMode2D.Impulse);

    }

    public void SetDir(Vector2 dir)
    {
        bulletDir = dir.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
