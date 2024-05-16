using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public enum CardType
    {
        Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, A
    }

    public float speed;
    public CardStats cardSO;
    public CardType cardType;
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



    }

    void MeleeAttack()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        dir = (mousePosition - transform.position).normalized;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
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
        Gizmos.DrawLine(transform.position,transform.position + dir);
    }
}
