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

    private void Awake()
    {
        animator = handle.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetStat(CharacterStats.Stats selfStats)
    {
        speed = selfStats.finalSpeed;
        coolTime= selfStats.finalCoolTime;
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
        switch (curSO.attackType)
        {
            case CardStats.AttackType.Melee:
                MeleeAttack();
                break;
            case CardStats.AttackType.Range:
                RangeAttack(true, curSO);
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
                Attack.Inst.shootRevolver(_dir, transform, shootPoint, curSO.bulletCount, true);
            else
                Attack.Inst.shootShotgun(_dir, transform, shootPoint, curSO.bulletCount, true);
        }
    }

    public virtual void MagicAttack(CardStats curSO)
    {
        if ( curCoolTime <= 0)
        {
            curCoolTime = coolTime;
            attackCoolImage.fillAmount = 1;
            animator.SetTrigger("SwordAttack");

            switch (TypeManager.Inst.playerCurSO.cardType)
            {
                case CardStats.CardType.Queen:
                    Attack.Inst.QueenMagic(shootPoint, curSO.bulletCount, true, opponent);
                    break;
                case CardStats.CardType.King:
                    Attack.Inst.KingMagic(_dir, transform, shootPoint, curSO.bulletCount, true);
                    break;
            }

        }
    }
}
