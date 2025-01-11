using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;
using UnityEngine.UI;
public enum NpcType {Npc,Player }

public class LobbyInteraction : MonoBehaviour
{

    private Animator animator;
    private SpriteRenderer spr;
    public float detectRange;
    public NpcType npcType;
    public bool inDetect = false;
    protected Action interactAction;

    private Vector3 angleVec;

    private bool isNear = false;

    [SerializeField] private float speed = 4f;


    [Tab("Mobile")] [SerializeField] bool mobileVersion;
    [SerializeField] VariableJoystick moveJoyStick, dirJoyStick;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
        interactAction += () => InteractAction();
    }

    public void DetectPlayer()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectRange);
        bool getPlayer = false;
        foreach (var col in colliders)
        {
            if (col.TryGetComponent(out LobbyInteraction lobbyNpc) && lobbyNpc != this &&
                lobbyNpc.npcType == NpcType.Player)
            {
                getPlayer = true;
                if (!isNear)
                {
                    NearAction();
                    isNear = true;
                }

            }
        }

        if (!getPlayer && isNear)
        {
            isNear = false;
            DetectOutPlayer();
        }

    }

    protected virtual void DetectOutPlayer(){}

    public void DetectNpc()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectRange);
        List<LobbyInteraction> npcs = new List<LobbyInteraction>();
        foreach (var col in colliders)
        {
            if (col.TryGetComponent(out LobbyInteraction lobbyNpc) && lobbyNpc != this)
            {
                npcs.Add(lobbyNpc);
            }
        }

        if (npcs.Count > 0)
        {
            float minDistance = Mathf.Infinity;
            LobbyInteraction nearNpc = npcs[0];
            foreach (var npc in npcs)
            {
                float _distance = Vector3.Distance(npc.transform.position, transform.position);
                if (_distance < minDistance)
                {
                    minDistance = _distance;
                    nearNpc = npc;
                }
                    
            }
            
            
            if(Input.GetKeyDown(KeyCode.V) && npcType == NpcType.Player)
                nearNpc.InteractAction();
        }
    }
    
    protected virtual void InteractAction(){}

    protected virtual void NearAction(){}

    private void LateUpdate()
    {
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -8, 8), Mathf.Clamp(transform.position.y, -4.2f, 1.1f));
    }
    protected void Move()
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

            if (animator)
            {
                if (x == 0 && y == 0) animator.SetBool("isWalk", false);
                else animator.SetBool("isWalk", true);
            }

            if (spr)
            {
                if (x < 0) spr.flipX = true;
                else if (x > 0) spr.flipX = false;
            }
            


            angleVec = new Vector3(x, y, 0).normalized;

            float moveX = angleVec.x * speed * Time.deltaTime;
            float moveY = angleVec.y * speed * Time.deltaTime;
            transform.Translate(new Vector3(moveX, moveY, 0), Space.World);
        }
      
    }

    private void OnDrawGizmos()
    {
        if (npcType == NpcType.Player)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position,detectRange);
        }
    }
}
