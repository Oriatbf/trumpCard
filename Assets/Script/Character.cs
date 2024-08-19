using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class Character : MonoBehaviour
{
    [Tab("Input")]
    public Image attackCoolImage;
    public float dashMaxCharging, dashSpeed,dashCool,dashInvTime;
    [HideInInspector]public float curCharging,curDashCool;
    public GameObject[] weapons;
    public GameObject handle;
    public CardStats characterSO;
    public RelicSkills relicSkills;
    public float flooringCool;
    public Transform shootPoint;
    public bool isPlayer;
    public float goldValue;

    [Tab("Debug")]
    public Vector3 _dir;
    public bool isFlooring;

     public float coolTime, curCoolTime,speed;
    [HideInInspector] public Animator animator;
     public Transform opponent;
    [HideInInspector]public Health health;
    [HideInInspector]public DashEffect dashEffect;
    [HideInInspector] public bool isDashing;
    Rigidbody2D rigid;



    private void Awake()
    {
        characterSO.relicInfor.characterTrans = transform;
        rigid = GetComponent<Rigidbody2D>();
        animator = handle.GetComponent<Animator>();
        health= GetComponent<Health>();
        dashEffect = GetComponent<DashEffect>();
        relicSkills = GetComponent<RelicSkills>();
        curCharging = dashMaxCharging;

        characterSO.relicInfor.characterHealth= health;
    }

    public void StartRelicSkill()
    {
        relicSkills.StartSkill();
        relicSkills.StartRatioSkill();
        if(isPlayer) relicSkills.SetRelicIcon();
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(curDashCool> 0) curDashCool -= Time.deltaTime;
    }
    private void LateUpdate()
    {
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -8, 8), Mathf.Clamp(transform.position.y, -4.2f, 3));
    }


    public void SetStat()
    {
        speed = characterSO.infor.speed;
        coolTime= characterSO.infor.coolTime;
        curCoolTime= characterSO.infor.coolTime;
        health.ResetHp(characterSO.infor.hp);
        
    }

   

    public void SetWeapon(int index)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
            if (i == index)
            {
                weapons[i].SetActive(true);
            }
        }
    }

    public void CoolTime(CardStats curSO)
    {
        if (curCoolTime > 0)
        {
            attackCoolImage.fillAmount = curCoolTime / coolTime;
            curCoolTime -= Time.deltaTime;
        }
        else
        {
            ChangeType(curSO);
        }
    }

    public void ChangeType(CardStats curSO)
    {
        switch (curSO.infor.attackType)
        {
            case CardStats.AttackType.Melee:
                MeleeAttack(false);
                break;
            case CardStats.AttackType.MeleeSting:
                MeleeAttack(true);
                break;
            case CardStats.AttackType.Range:
                RangeAttack(true, curSO);
                break;
            case CardStats.AttackType.Bow:
                BowAttack();
                break;
            case CardStats.AttackType.ShotGun:
                RangeAttack(false, curSO);
                break;
            case CardStats.AttackType.Magic:
                MagicAttack(curSO);
                break;
        }
    }

   public void MeleeAttack(bool isSting)
    {
        if (Input.GetMouseButtonDown(0) && curCoolTime <= 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.position + _dir, 1.5f);
            curCoolTime = coolTime;
            attackCoolImage.fillAmount = 1;
            if (!isSting) animator.SetTrigger("SwordAttack");
            else animator.SetTrigger("StingAttack");

        }
    }

    public virtual void RangeAttack(bool isRevolver,CardStats curSO)
    {
        if (curCoolTime <= 0)
        {
            curCoolTime = coolTime;
            attackCoolImage.fillAmount = 1;
            animator.SetTrigger("GunAttack");
            if (isRevolver)
                Attack.Inst.shootRevolver(_dir, handle.transform.parent, shootPoint, curSO, isPlayer);
            else
                Attack.Inst.shootShotgun(_dir, handle.transform.parent, shootPoint, curSO, isPlayer);
        }
    }

    public virtual void MagicAttack(CardStats curSO)
    {
        if ( curCoolTime <= 0)
        {
            curCoolTime = coolTime;
            attackCoolImage.fillAmount = 1;
            animator.SetTrigger("SwordAttack");

            switch (curSO.infor.cardType)
            {
                case CardStats.CardType.Queen:
                    Attack.Inst.QueenMagic(shootPoint, curSO, true, opponent);
                    break;
                case CardStats.CardType.King:
                    Attack.Inst.KingMagic(_dir, handle.transform.parent, shootPoint, curSO, isPlayer);
                    break;
            }

        }
    }
    /*
    public virtual void BowCharge()
    {
        float curCharge = 0;
        float damage = 0;
        curCharge  += Time.deltaTime;
        if(curCharge>=characterSO.infor.maxCharge) curCharge = characterSO.infor.maxCharge;
        for(int i = 0; i < characterSO.infor.chargeStep.Count; i++)
        {
            if(curCharge < characterSO.infor.chargeStep[i])
            {
                damage = characterSO.infor.chargeDamage[i];
                break;
            }
        }
    }*/


    public virtual void BowAttack()
    {
       
    }

    public void FlooringDamage()
    {
        if(flooringCool <= 0 && isFlooring)
        {
            health.OnDamage(2);
            flooringCool = 5f;
        }
        else
        {
            flooringCool-=Time.deltaTime;
        }
    }

    public void DashMove(Vector2 dir)
    {
        curCharging--;
        isDashing = true;
        curDashCool = dashCool;
        health.InvTime(dashInvTime);
        rigid.velocity = dir * dashSpeed;
        dashEffect.ActiveDashEffect(0.2f);
        DOVirtual.DelayedCall(0.2f, () =>
        {
            rigid.velocity = Vector2.zero;
            isDashing= false;

            });
    }

}
