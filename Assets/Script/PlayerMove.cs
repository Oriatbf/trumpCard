using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerMove : MonoBehaviour
{

  

    public float speed;
   
    
    private float angle;
    private Vector3 dir;

    public GameObject test;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        transform.position = Vector2.MoveTowards(transform.position,new Vector2(transform.position.x+x, transform.position.y+y),speed*Time.deltaTime);

       
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        
        MeleeAttack();


    }

    void MeleeAttack()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        dir = (mousePosition - transform.position).normalized;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (Input.GetKeyDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.position + dir, 1.5f);
            if (hit.collider!=null)
            {
                hit.transform.GetComponent<Health>().OnDamage(10f);
            }
        }
    }

    void RangeAttack()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dir = (mousePosition - transform.position).normalized;
            //발사
        }
        
    }

    void MagicAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,transform.position + dir*1.5f);
        Gizmos.DrawWireCube(transform.position + transform.forward,transform.forward);
    }
}
