using Cinemachine;
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
    private float dashCool = 5;
    private float dashMaxCharging = 1;
    public float  dashSpeed,dashInvTime;
    [HideInInspector]public float curCharging,curDashCool;
    public GameObject[] weapons;
    public GameObject handle;
    public CardStats characterSO;
    public RelicSkills relicSkills;
    public float flooringCool;
    public Transform shootPoint;
    public bool isPlayer;
    public float goldValue;
    public LayerMask opponentMask;
    [SerializeField] CardStats resetRelicInfor;
    [HideInInspector] public float extraAttackRatio = 1;

    [Tab("Debug")]
    public Vector3 _dir;
    public bool isFlooring;

    public float coolTime, curCoolTime,speed;
    public Transform opponent;

    [HideInInspector] public bool isDashing;
     public float _curCharging; 

    [HideInInspector] public Animator animator; 
    [HideInInspector]public Health health;
    [HideInInspector] public GambleGauge gambleGauge;
    [HideInInspector]public DashEffect dashEffect;

   
    Rigidbody2D rigid;



    public virtual void Awake()
    {

       
       
        rigid = GetComponent<Rigidbody2D>();
        animator = handle.GetComponent<Animator>();
        health= GetComponent<Health>();
        dashEffect = GetComponent<DashEffect>();
        relicSkills = GetComponent<RelicSkills>();
        gambleGauge= GetComponent<GambleGauge>();
        curCharging = 1;
        characterSO.relicInfor.characterTrans = transform;
        characterSO.relicInfor.characterHealth= health;

        int randomGold = Random.Range(100, 151);
        goldValue= randomGold;
    }

    public void Gambling()
    {
        characterSO.relicInfor = resetRelicInfor.relicInfor;
        characterSO.relicInfor.characterTrans = transform;
        characterSO.relicInfor.characterHealth = health;
        gambleGauge._curGauge = 0;
        TypeManager.Inst.TypeChange(GambleManager.GambleIndex(), transform, isPlayer, characterSO);
       
        StartRelicSkill();
        SetStat();
    }

    public void StartRelicSkill()
    {
        characterSO.relicInfor.relicPlusHealth = 0;
        relicSkills.StartSkill();

        if(isPlayer) relicSkills.SetRelicIcon();
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        Gambling();
       
       
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (curDashCool > 0 && curCharging <1)
        {
            curDashCool -= Time.deltaTime;
        }
        if (curDashCool <= 0 && curCharging<1)
        {
            curCharging++;

        }

        if (gambleGauge._curGauge >= gambleGauge.maxGauge)
        {
            Gambling();
        }
    }
    private void LateUpdate()
    {
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -8, 8), Mathf.Clamp(transform.position.y, -4.2f, 3));
    }


    public void SetStat()
    {
        speed = characterSO.infor.speed;
        coolTime= characterSO.infor.coolTime < 0.1f?0.1f: characterSO.infor.coolTime; //쿨타임 최소치
        curCoolTime= coolTime;
        characterSO.infor.damage = characterSO.infor.damage<1?1:characterSO.infor.damage; // 데미지 최소치
        health.ResetHp(characterSO.infor.hp);
        health.SetHp(characterSO.relicInfor.remnantHealth);
        if(health.curHp + characterSO.relicInfor.relicPlusHealth <=0) health.curHp = 1;
        else health.curHp += characterSO.relicInfor.relicPlusHealth;
        health.HpBarIncrease();


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

   public virtual void MeleeAttack(bool isSting)
    {
        if ( curCoolTime <= 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.position + _dir, 1.5f);
            curCoolTime = coolTime;
            attackCoolImage.fillAmount = 1;

            if (!isSting)
            {
                animator.SetTrigger(_dir.x < 0 ? "SwordFlipAttack" : "SwordAttack"); // 역방향 재생


            }
            else animator.SetTrigger("StingAttack");

        }
    }

    public void MeleeDamage()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, _dir, 2f, opponentMask);
        if (hit.collider != null)
        {
            hit.transform.GetComponent<Health>().OnDamage(Critical(characterSO,characterSO.infor.damage * extraAttackRatio));
        }
        AudioManager.Inst.AudioEffectPlay(characterSO.infor.cardNum);
    }

    public float Critical(CardStats charSO, float damage)
    {
        int a = Random.Range(1, 101);
        if (charSO.relicInfor.criticalChance >= a)
        {
            return damage * charSO.relicInfor.criticalDamage;
        }
        else return damage;
    }



    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + transform.forward, transform.forward);
        Gizmos.DrawRay(transform.position, _dir * 2);

        Gizmos.color = Color.green;

    }


    public virtual void RangeAttack(bool isRevolver,CardStats curSO)
    {
        if (curCoolTime <= 0)
        {
            curCoolTime = coolTime;
            attackCoolImage.fillAmount = 1;
            animator.SetTrigger("GunAttack");
            if (isRevolver)
                Attack.Inst.shootRevolver(_dir, handle.transform.parent, shootPoint, curSO, isPlayer,this);
            else
                Attack.Inst.shootShotgun(_dir, handle.transform.parent, shootPoint, curSO, isPlayer,this);
        }
    }

    public virtual void MagicAttack(CardStats curSO)
    {
        if ( curCoolTime <= 0)
        {
            curCoolTime = coolTime;
            attackCoolImage.fillAmount = 1;
            animator.SetTrigger(_dir.x < 0 ? "SwordFlipAttack" : "SwordAttack");

            switch (curSO.infor.cardType)
            {
                case CardStats.CardType.Queen:
                    Attack.Inst.QueenMagic(shootPoint, curSO, isPlayer, opponent,this);
                    break;
                case CardStats.CardType.King:
                    Attack.Inst.KingMagic(_dir, handle.transform.parent, shootPoint, curSO, isPlayer, this);
                    break;
            }

        }
    }
 


    public virtual void BowAttack()
    {
        _curCharging += Time.deltaTime;
        if (_curCharging >= coolTime) _curCharging = coolTime;
        attackCoolImage.fillAmount = _curCharging / coolTime;
        if(_curCharging >= coolTime) BowShoot();
    }

    public virtual void BowShoot()
    {
        bool maxCharging = _curCharging >= coolTime;
        Attack.Inst.shootBow(_dir, handle.transform.parent, shootPoint, characterSO, isPlayer, maxCharging,this);
        attackCoolImage.fillAmount = 0;
        _curCharging = 0;
    }

    public virtual void BowShoot(Vector3 dir)
    {
        bool maxCharging = _curCharging >= coolTime;
        Attack.Inst.shootBow(dir, handle.transform.parent, shootPoint, characterSO, isPlayer, maxCharging, this);
        attackCoolImage.fillAmount = 0;
        _curCharging = 0;
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
