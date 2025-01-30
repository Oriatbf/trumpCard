using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class Health : MonoBehaviour
{
    public CharacterType characterType;
     [Tab("Hp")]
    //Hp Value
    public float curHp;
    public float maxHp;
    
    public Image hpBar;
    public Material whiteMaterial,defaultMaterial;
    [HideInInspector]public SpriteRenderer spr;
    public Rigidbody2D rigid;
    
    public bool isInv = false;

    public Action OnDamage,OnHeal,OnAttack;
    
    

    protected virtual void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        curHp = 100;
        maxHp = 100;
    }

    protected virtual void Start()
    {
        hpBar.fillAmount = 1;
        SetAction();
    }

    protected virtual void SetAction()  //액션 인풋
    {
        OnDamage += () => HpBarIncrease();
        //OnDamage += () => GameManager.Inst.GetOpponent(character).unitHealth.OnAttack?.Invoke();
        OnHeal += () => HpBarIncrease();
    }


    public void ResetHp(float maxHp,float curHp) //Hp 리셋 설정
    {
        this.maxHp = maxHp;
        this.curHp = curHp;
        if(this.curHp > this.maxHp)
            this.curHp = this.maxHp;
        HpBarIncrease();
    }

    public virtual void GetDamage(float damage)
    {
        if (!isInv )
        {
            if (whiteMaterial && defaultMaterial)
            {
                spr.material = whiteMaterial;
                DOVirtual.DelayedCall(0.1f, () => spr.material = defaultMaterial);
            }
           
            curHp -= damage;
            if(damage >=0) UIManager.Inst.DamageUI(null,transform,damage); //데미지 UI 표시


            if (curHp <= 0)
            {
                Destroy(gameObject);
            }

            OnDamage?.Invoke();

        }
       
    }

    public virtual void OnRecorvery(float healAmount)
    {
        UIManager.Inst.RecorveryUI(null,transform,healAmount); //heal 숫자 UI 
        curHp += healAmount;
        if(curHp > maxHp) curHp = maxHp;
        OnHeal?.Invoke();
    }

    public void HpBarIncrease() => hpBar.fillAmount = curHp / maxHp;
    
    public void InvTime(float time)
    {
        isInv = true;
        DOVirtual.DelayedCall(time, () => isInv = false);
        //character.GetForce(bulletDir,5,0.05f);
    }

    public virtual void GetForce(Vector2 dir,float forcePower,float time,Action action = null)
    {
        if (rigid)
        {
            rigid.linearVelocity = dir * forcePower;
            DOVirtual.DelayedCall(time, () =>
            {
                rigid.linearVelocity = Vector2.zero;
                action?.Invoke();
            });
        }
      
    }
    


    
}

