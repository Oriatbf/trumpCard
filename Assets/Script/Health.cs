using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class Health : MonoBehaviour
{
     [Tab("Hp")]
    //Hp Value
    public float curHp;
    public float maxHp;
    
    [SerializeField] Image hpBar;
    [SerializeField] Material whiteMaterial,defaultMaterial;
    private SpriteRenderer spr;
    public bool isInv = false;

    public Action OnDamage,OnHeal,OnAttack;
    

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        curHp = 100;
        maxHp = 100;
    }

    private void Start()
    {
        hpBar.fillAmount = 1;
        SetAction();
    }

    void SetAction()  //액션 인풋
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

    public void GetDamage(float damage)
    {
        if (!isInv )
        {
            spr.material = whiteMaterial;
            DOVirtual.DelayedCall(0.1f, () => spr.material = defaultMaterial);
            curHp -= damage;
            if(damage >=0) UIManager.Inst.DamageUI(null,transform,damage); //데미지 UI 표시


            if (curHp <= 0)
            {
                Destroy(gameObject);
            }

            OnDamage?.Invoke();

        }
       
    }

    public void OnRecorvery(float healAmount)
    {
        UIManager.Inst.RecorveryUI(null,transform,healAmount); //heal 숫자 UI 
        curHp += healAmount;
        if(curHp > maxHp) curHp = maxHp;
        OnHeal?.Invoke();
    }

    private void HpBarIncrease() => hpBar.fillAmount = curHp / maxHp;
    
    public void InvTime(float time)
    {
        isInv = true;
        DOVirtual.DelayedCall(time, () => isInv = false);
    }
    


    
}

