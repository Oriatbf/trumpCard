using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    public float coolTime,curCoolTime;
    [SerializeField] Image attackCoolImage;
    [SerializeField] GameObject bullet;

    [SerializeField] Transform shootPoint;

    
    private float angle;
    private Vector3 dir;

    public GameObject test;

    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        switch (PlayerTypeManager.Inst.attackType)
        {
            case PlayerTypeManager.AttackType.Melee:
                MeleeAttack();
                break;
            case PlayerTypeManager.AttackType.Range:
                RangeAttack();
                break;
        }
       
        Move();
       

        if(curCoolTime> 0)
        {
            attackCoolImage.fillAmount = curCoolTime / coolTime;
            curCoolTime-= Time.deltaTime;
        }

    }

    private void Move()
    {
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + x, transform.position.y + y), speed * Time.deltaTime);
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            dir = (mousePosition - transform.position).normalized;
            angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        }
    }

    void MeleeAttack()
    { 
        if (Input.GetMouseButtonDown(0)&& curCoolTime <= 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.position + dir, 1.5f);
            curCoolTime = coolTime;
            attackCoolImage.fillAmount = 1;
            animator.SetTrigger("SwordAttack");
            if (hit.collider!=null)
            {
                //hit.transform.GetComponent<Health>().OnDamage(10f);
            }
        }
    }

    void RangeAttack()
    {
        if (Input.GetMouseButtonDown(0) && curCoolTime <= 0)
        {
              
            curCoolTime = coolTime;
            attackCoolImage.fillAmount = 1;
            animator.SetTrigger("GunAttack");
            ObjectPoolingManager.Inst.shootRevolver(dir);
              
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
