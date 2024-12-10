using System;
using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using OneLine.Examples;
using UnityEngine;
using UnityEngine.UI;
using VInspector;
using Random = UnityEngine.Random;

public enum CharacterType
{
    Player,Enemy
}

public class Character : MonoBehaviour
{
    

    [Tab("Input")] 
    public CharacterType characterType;
    public Image attackCoolImage;
    private float dashCool = 5;
    private float dashMaxCharging = 1;
    public float  dashSpeed,dashInvTime;
    [HideInInspector]public float curCharging,curDashCool;
    public GameObject[] weapons;
    public GameObject handle;
    public RelicSkills relicSkills;
    public Transform shootPoint;
    public float goldValue;

    [Tab("Debug")]
    public Vector3 _dir;
    protected float _angle;
    
    public Character opponent;
    public float curCoolTime,coolTime;
    [HideInInspector] public bool isDashing;
     public float _curCharging; 

    [HideInInspector] public Animator animator; 
    [HideInInspector]public Health health;
    [HideInInspector] public GambleGauge gambleGauge;
    [HideInInspector]public DashEffect dashEffect;

   
    private Rigidbody2D rigid;
    private SpriteRenderer spr;
    public Stat stat;
    public ShootInfor shootInfor;



    public virtual void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        animator = handle.GetComponent<Animator>();
        health= GetComponent<Health>();
        dashEffect = GetComponent<DashEffect>();
        relicSkills = GetComponent<RelicSkills>();
        gambleGauge= GetComponent<GambleGauge>();
        curCharging = 1;
      //  characterSO.relicInfor.characterTrans = transform;
      //  characterSO.relicInfor.characterHealth= health;

        int randomGold = Random.Range(100, 151);
        goldValue= randomGold;
    }

    public void Gambling() //갬블시 초기화 되는 내용 및 체인지
    {
        gambleGauge._curGauge = 0;
        StartRelicSkill();
        SetWeapon(stat.cardNum);
        SetStat();
       
    }

    public void CardStatReset() //게임이 다끝나고 로비로 돌아가야할 때 실행
    {
        //characterSO.ResetRelicInfor();
       // characterSO.ClearDebuffList();
    }

    public void StartRelicSkill()
    {
        //relicSkills.StartSkill();

       // if(characterType == CharacterType.Player) relicSkills.SetRelicIcon();
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
        CoolTime();
        shootInfor.dir = _dir;
    }
    private void LateUpdate()
    {
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -8, 8), Mathf.Clamp(transform.position.y, -4.2f, 3));
    }


    public void SetStat()
    {
        float hpRatio = health.curHp/health.maxHp; //hp 비율
        stat.statValue = CardDataManager.Inst.RandomCard().stat.statValue;
        Debug.Log(stat.statValue.hp);
        stat.StatUpAction();
        
        var basicStat = stat.statValue;
        basicStat.speed = basicStat.speed <1?1:basicStat.speed;
        basicStat.coolTime= basicStat.coolTime < 0.1f?0.1f: basicStat.coolTime; //쿨타임 최소치
        basicStat.damage = basicStat.damage<1?1:basicStat.damage; // 데미지 최소치
        
        curCoolTime= basicStat.coolTime;
        coolTime = basicStat.coolTime;
        health.ResetHp(basicStat.hp,(basicStat.hp * hpRatio)+10);

        shootInfor = new ShootInfor(this, stat);
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

    public void CoolTime()
    {
        if (curCoolTime > 0)
        {
            attackCoolImage.fillAmount = curCoolTime / stat.statValue.coolTime;
            curCoolTime -= Time.deltaTime;
        }
        else
        {
            ChangeType(stat);
        }
    }

    public void ChangeType(Stat stat)
    {
        switch (stat.cardRole)
        {
            case CardRole.Melee:
                MeleeAttack(false);
                break;
            case CardRole.MeleeSting:
                MeleeAttack(true);
                break;
            case CardRole.Range:
                RangeAttack(true);
                break;
            case CardRole.Bow:
                BowAttack();
                break;
            case CardRole.ShotGun:
                RangeAttack(false);
                break;
            case CardRole.Magic:
                MagicAttack();
                break;
        }
    }

   public virtual void MeleeAttack(bool isSting)
    {
        if ( curCoolTime <= 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.position + _dir, 1.5f);
            curCoolTime = stat.statValue.coolTime;
            attackCoolImage.fillAmount = 1;

            if (!isSting)
            {
                animator.SetTrigger(_dir.x < 0 ? "SwordFlipAttack" : "SwordAttack"); // 역방향 재생
                //if(characterSO.relicInfor.isSlash) Attack.Inst.Slash(shootInfor);
             
            }
            else animator.SetTrigger("StingAttack");

        }
    }

    public void MeleeDamage()
    {
        if(stat.cardType == CardType.Melee)
        {
           // EffectManager.Inst.SpawnEffect(shootPoint.transform, 1, Quaternion.Euler(0, 0, _angle));
           // Attack.Inst.Slash(shootInfor);
        }
           

       // RaycastHit2D hit;
      //  hit = Physics2D.Raycast(transform.position, _dir, 2f, opponentMask);
      /*
      if (hit.collider != null)
      {
          Health opponentHealth = hit.transform.GetComponent<Health>();
          foreach (Debuff debuff in characterSO.debuffs)
          {
              debuff.Apply(opponentHealth);
          }
          opponentHealth.OnDamage(Critical(stat));

      }*/
      
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, _dir, 2f);
        foreach (var hit in hits)
        {
            if (hit.transform.TryGetComponent(out Character _character))
            {
                if (_character.characterType != characterType)
                {
                    _character.health.GetDamage(Critical.CriticalChance(stat));
                }
            }
        }
       
        AudioManager.Inst.AudioEffectPlay(stat.cardNum);
    }




    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + transform.forward, transform.forward);
        Gizmos.DrawRay(transform.position, _dir * 2);

        Gizmos.color = Color.green;

    }


    public virtual void RangeAttack(bool isRevolver)
    {
        if (curCoolTime <= 0)
        {
            
            curCoolTime = coolTime;
            attackCoolImage.fillAmount = 1;
            animator.SetTrigger("GunAttack");
            Attack.Inst.Shoot(shootInfor);
        }
    }

    public virtual void MagicAttack()
    {
        if ( curCoolTime <= 0)
        {
            curCoolTime = coolTime;
            attackCoolImage.fillAmount = 1;
            animator.SetTrigger(_dir.x < 0 ? "SwordFlipAttack" : "SwordAttack");
/*
            switch (curSO.infor.cardType)
            {
                case CardStats.CardType.Queen:
                   // Attack.Inst.QueenMagic(shootPoint, curSO, isPlayer, opponent,this);
                    break;
                case CardStats.CardType.King:
                    Attack.Inst.Shoot(shootInfor);
                    break;
            }*/

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
        Attack.Inst.Shoot(shootInfor,maxCharging);
        attackCoolImage.fillAmount = 0;
        _curCharging = 0;
    }

    public virtual void BowShoot(Vector3 dir)
    {
        bool maxCharging = _curCharging >= coolTime;
        Attack.Inst.Shoot(shootInfor, maxCharging);
        attackCoolImage.fillAmount = 0;
        _curCharging = 0;
    }
    
    public void DashMove(Vector2 dir)
    {
        curCharging--;
        isDashing = true;
        curDashCool = dashCool;
        health.InvTime(dashInvTime);
        dashEffect.ActiveDashEffect(0.2f);
        GetForce(dir,dashSpeed,0.2f,()=>isDashing = false);
    }

    public void GetForce(Vector2 dir,float forcePower,float time,Action action = null)
    {
        rigid.linearVelocity = dir * forcePower;
        DOVirtual.DelayedCall(time, () =>
        {
            rigid.linearVelocity = Vector2.zero;
            action?.Invoke();
        });
    }

}
