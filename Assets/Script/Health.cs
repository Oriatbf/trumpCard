using DamageNumbersPro;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    //Hp Value
    public float curHp;
    public float maxHp;
    [SerializeField] DamageNumber numberPrefab;
    [SerializeField] RectTransform rectParent;
    [SerializeField] float floorTickTime =1;
    [SerializeField] Image hpBar;
    [HideInInspector] public bool autoHeal;
    [HideInInspector] public float autoHealSpeed;
    [HideInInspector] public bool isFloor;
    bool isInv = false;

    //GambleGauge Value
    GambleGauge gambleGauge;
    [SerializeField] float hittedValue,attackValue; //hitted : 맞았을 때 , attack : 공격했을 때 늘어날 값

    Character character;

    private void Start()
    {
        gambleGauge=GetComponent<GambleGauge>();
        character= GetComponent<Character>();
        hpBar.fillAmount = 1;
    }

    private void Update()
    {
        if(autoHeal)AutoHeal();
    }

    public void SetHp(float statHp)
    {
        float overHp = 0; 
        if(maxHp < statHp)
        {
            overHp  = statHp - maxHp;
        }
       
        maxHp = statHp;
        curHp += overHp;
        if(curHp > maxHp) curHp= maxHp;

    }

    public void ResetHp(float maxHp)
    {
        this.maxHp = maxHp;
        curHp = maxHp;
        HpBarIncrease();
    }

    public void AutoHeal()
    {
        if (curHp <= maxHp)
        {
            curHp += autoHealSpeed * Time.deltaTime;
            HpBarIncrease();
        }
    
    }

    public void OnDamage(float damage)
    {
        if (!isInv)
        {
            curHp -= damage;
            HpBarIncrease();
            IncreaseGambleGauge(true);
            character.opponent.GetComponent<Health>().IncreaseGambleGauge(false); // 적 캐릭터 공격 시 올라가는 갬블게이지
            DamageNumber damageNumber = numberPrefab.SpawnGUI(rectParent,transform.position,damage);
            if (curHp <= 0)
            {
                curHp = maxHp;
                //사망
            }
        }
       
    }

    public void IncreaseGambleGauge(bool isHitted)
    {
        if (isHitted) gambleGauge.IncreaseGambleGauge(hittedValue);
        else gambleGauge.IncreaseGambleGauge(attackValue);
    }

    public void OnHeal(float healAmount)
    {
        curHp += healAmount;
        if(curHp > maxHp)
            curHp = maxHp;
        HpBarIncrease();
    }

    public void HpBarIncrease()
    {
        hpBar.fillAmount = curHp/maxHp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckFlooring(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        CheckFlooring(collision);
    }

    public void InvTime(float time)
    {
        isInv= true;
        DOVirtual.DelayedCall(time, () => isInv = false);
    }

    void CheckFlooring(Collider2D collision)
    {
        if (collision.CompareTag("Floor") && character.isPlayer != collision.GetComponent<FlooringCol>().isPlayerOwner && !isFloor)
        {
            isFloor = true;
            OnDamage( collision.GetComponent<FlooringCol>().tickDamage);
            DOVirtual.DelayedCall(floorTickTime, () => isFloor = false);
        }
    }
}
