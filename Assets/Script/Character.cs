using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [HideInInspector] public float coolTime, curCoolTime,speed;
    public Image attackCoolImage;
    public GameObject[] weapons;
    public GameObject handle;
    [HideInInspector] public Animator animator;
     public Vector3 _dir;
    [HideInInspector] public Transform opponent;
    public Transform shootPoint;
    public bool isPlayer;
    public CardStats characterSO;
    [HideInInspector]public Health health;
    public bool isFlooring;
    public float flooringCool;

    private void Awake()
    {
        animator = handle.GetComponent<Animator>();
        health= GetComponent<Health>();
        characterSO.relicInfor.characterHealth= health;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
                MeleeAttack();
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

   public void MeleeAttack()
    {
        if (Input.GetMouseButtonDown(0) && curCoolTime <= 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.position + _dir, 1.5f);
            curCoolTime = coolTime;
            attackCoolImage.fillAmount = 1;
            animator.SetTrigger("SwordAttack");
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
                Attack.Inst.shootRevolver(_dir, transform, shootPoint, curSO, isPlayer);
            else
                Attack.Inst.shootShotgun(_dir, transform, shootPoint, curSO, isPlayer);
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
                    Attack.Inst.KingMagic(_dir, transform, shootPoint, curSO, isPlayer);
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
}
