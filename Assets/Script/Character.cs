using System;
using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using OneLine.Examples;
using UnityEngine;
using UnityEngine.Serialization;
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
    public Image attackCoolImage;
    private float dashCool = 5;
    private float dashMaxCharging = 1;
    public float  dashSpeed,dashInvTime;
    [HideInInspector]public float curCharging,curDashCool;
    public GameObject[] weapons;
    public GameObject handle;
    public Transform shootPoint;
    public int goldValue;

    [Tab("Debug")]
    public Vector3 _dir;
    public float _angle;
    
    public Character opponent;
    public List<StatusEffect> debuffs = new List<StatusEffect>();
    public float curCoolTime;
    private float _coolTime;

    public float coolTime
    {
        get
        {
            _coolTime = stat.FinalValue().coolTime;
            if (_coolTime < 0.1f)
                return 0.1f;
            else return _coolTime;
        }
        
    }
    
    [HideInInspector] public bool isDashing;
     public float _curCharging; 

    [HideInInspector] public Animator animator; 
    [FormerlySerializedAs("health")] [HideInInspector]public UnitHealth unitHealth;
    [HideInInspector] public GambleGauge gambleGauge;
    [HideInInspector]public DashEffect dashEffect;

   
    private Rigidbody2D rigid;
    private SpriteRenderer spr;
    public Stat stat;
    public ShootInfor shootInfor;
    private PossessionRelics possessionRelics;
    public CreatureCustody creatureCustody;



    public virtual void Awake()
    {
        possessionRelics = GetComponent<PossessionRelics>();
        creatureCustody = GetComponent<CreatureCustody>();
        stat.statUpAction = null;
        spr = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        animator = handle.GetComponent<Animator>();
        unitHealth= GetComponent<UnitHealth>();
        dashEffect = GetComponent<DashEffect>();
        gambleGauge= GetComponent<GambleGauge>();
       
        if (creatureCustody == null)
        {
            Debug.LogError("creatureCustody가 없음");
        }
        curCharging = 1;

        int randomGold = Random.Range(100, 151);
        goldValue= randomGold;
    }

    public void Gambling() //갬블시 초기화 되는 내용 및 체인지
    {
        SetStat();
    }
    
    
    public virtual void Start()
    {
        possessionRelics.ExcuteRelic();
        stat.statUpAction += () => creatureCustody.summon();
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

      
        CoolTime();
        shootInfor.dir = _dir;
        shootInfor.angle = _angle;
    }
    private void LateUpdate()
    {
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -8, 8), Mathf.Clamp(transform.position.y, -4.2f, 3));
    }

    

    protected virtual void Move()
    {
        
    }


    public void SetStat()
    {
        float hpRatio = unitHealth.curHp/unitHealth.maxHp; //hp 비율

        var randomStat = CardDataManager.Inst.RandomCard().stat;
        stat.originStatValue = randomStat.originStatValue;
        stat.cardNum = randomStat.cardNum;
        SetWeapon(stat.cardNum);
        stat.cardRole = randomStat.cardRole;
        stat.cardType = randomStat.cardType;
        string characterPath = unitHealth.characterType == CharacterType.Player ? "Card_Red_" : "Card_Blue_";
        unitHealth.spr.sprite = Resources.Load<Sprite>("Sprite/Character/Cards/" +characterPath +randomStat.cardName);
        stat.Action();
      
        
        var basicStat = stat.originStatValue;
        basicStat.speed = basicStat.speed <1?1:basicStat.speed;
        basicStat.coolTime= basicStat.coolTime < 0.1f?0.1f: basicStat.coolTime; //쿨타임 최소치
        basicStat.damage = basicStat.damage<1?1:basicStat.damage; // 데미지 최소치
        
        curCoolTime= basicStat.coolTime;
        unitHealth.ResetHp(basicStat.hp,(basicStat.hp * hpRatio));
        unitHealth.OnRecorvery(10);

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
            attackCoolImage.fillAmount = curCoolTime / stat.originStatValue.coolTime;
            curCoolTime -= Time.unscaledDeltaTime;
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
            curCoolTime = coolTime;
            attackCoolImage.fillAmount = 1;

            if (!isSting)
            {
                animator.SetTrigger(_dir.x < 0 ? "SwordFlipAttack" : "SwordAttack"); // 역방향 재생
               Attack.Inst.Slash(shootInfor);
             
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
      
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, _dir, 2f);
        foreach (var hit in hits)
        {
            if (hit.transform.TryGetComponent(out Health _health))
            {
                if (_health.characterType != unitHealth.characterType)
                {
                    _health.GetDamage(Critical.CriticalChance(stat));
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
 


    protected virtual void BowAttack()
    {
        _curCharging += Time.deltaTime;
        if (_curCharging >= coolTime) _curCharging = stat.FinalValue().coolTime;
        attackCoolImage.fillAmount = _curCharging / stat.FinalValue().coolTime;
        if(_curCharging >= coolTime) BowShoot();
    }

    protected virtual void BowShoot()
    {
        bool maxCharging = _curCharging >= coolTime;
        Attack.Inst.Shoot(shootInfor,maxCharging);
        attackCoolImage.fillAmount = 0;
        _curCharging = 0;
    }

    protected virtual void BowShoot(Vector3 dir)
    {
        bool maxCharging = _curCharging >= coolTime;
        Attack.Inst.Shoot(shootInfor, maxCharging);
        attackCoolImage.fillAmount = 0;
        _curCharging = 0;
    }
    
    protected void DashMove(Vector2 dir)
    {
        curCharging--;
        isDashing = true;
        curDashCool = dashCool;
        unitHealth.InvTime(dashInvTime);
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
