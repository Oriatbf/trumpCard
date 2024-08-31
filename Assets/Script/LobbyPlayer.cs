using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class LobbyPlayer : MonoBehaviour
{

    Animator animator;
    SpriteRenderer spr;
    [SerializeField] LobbyManager manager;
    [Tab("Debug")]
    [SerializeField] Vector3 angleVec;

    public float speed = 4f;


    [Tab("Mobile")]
    [SerializeField] bool mobileVersion;
    [SerializeField] VariableJoystick moveJoyStick, dirJoyStick;

    private void Awake()
    {
        animator= GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!TutorialManager.Inst.isTutorialing)
            Move();
    }

    private void LateUpdate()
    {
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -8, 8), Mathf.Clamp(transform.position.y, -4.2f, 1.1f));
    }
    private void Move()
    {
        if (!TutorialText.Inst.disableAction)
        {
            float x;
            float y;
            //Move
            if (mobileVersion)
            {
                x = moveJoyStick.Horizontal;
                y = moveJoyStick.Vertical;
            }
            else
            {
                x = Input.GetAxisRaw("Horizontal");
                y = Input.GetAxisRaw("Vertical");
            }

            if (x == 0 && y == 0) animator.SetBool("isWalk", false);
            else animator.SetBool("isWalk", true);

            if (x < 0) spr.flipX = true;
            else if (x > 0) spr.flipX = false;


            angleVec = new Vector3(x, y, 0).normalized;

            float moveX = angleVec.x * speed * Time.deltaTime;
            float moveY = angleVec.y * speed * Time.deltaTime;
            transform.Translate(new Vector3(moveX, moveY, 0), Space.World);
        }
      
    }
}
