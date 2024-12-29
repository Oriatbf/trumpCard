using System;
using DamageNumbersPro;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using VInspector;
using VInspector.Libs;

public class Health : MonoBehaviour
{
    [Tab("Hp")]
    //Hp Value
    public float curHp;
    public float maxHp;
    
    [SerializeField] private RectTransform rectParent;
    [SerializeField] Image hpBar;
    [SerializeField] Material whiteMaterial,defaultMaterial;
    [SerializeField] SpriteRenderer spr;
    public bool isInv = false;

    public Action OnDamage,OnHeal,OnAttack;

    //GambleGauge Value
    GambleGauge gambleGauge;
    private float hittedValue = 5f; 
    Character character;

    private void Awake()
    {
        curHp = 100;
        maxHp = 100;
    }

    private void Start()
    {
        gambleGauge=GetComponent<GambleGauge>();
        character= GetComponent<Character>();
        hpBar.fillAmount = 1;
       SetAction();


        //  if(!character.isPlayer)enemyMove=GetComponent<EnemyMove>();
    }

    void SetAction()
    {
        OnDamage += () => gambleGauge.IncreaseGambleGauge(hittedValue);
        OnDamage += () => HpBarIncrease();
        OnDamage += () => GameManager.Inst.GetOpponent(character).health.OnAttack?.Invoke();
        OnHeal += () => HpBarIncrease();
    }


    public void ResetHp(float maxHp,float curHp)
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
            if(damage >=0)
                UIManager.Inst.DamageUI(rectParent,transform,damage);


            if (curHp <= 0)
            {
                isInv = true;
                character.opponent.GetComponent<Health>().isInv = true;
                if (character.characterType == CharacterType.Player)
                {
                    StartCoroutine( GameManager.Inst.GameEnd(false, character));
                }
                else if (character.characterType == CharacterType.Enemy)
                {
                    StartCoroutine(GameManager.Inst.GameEnd(true, character));
                     TopUIController.Inst.GetGold(character.goldValue); 
                }
                return;
            }

            OnDamage?.Invoke();

        }
       
    }





    public void OnRecorvery(float healAmount)
    {
        UIManager.Inst.RecorveryUI(rectParent,transform,healAmount);
        curHp += healAmount;
        if(curHp > maxHp)
            curHp = maxHp;
        OnHeal?.Invoke();;
    }

    private void HpBarIncrease() => hpBar.fillAmount = curHp / maxHp;
    public void InvTime(float time)
    {
        isInv = true;
        DOVirtual.DelayedCall(time, () => isInv = false);
    }

    public void DotDamage(float time,float damage)
    {
        StartCoroutine( ApplyDotDamage(time, damage));
    }

    IEnumerator ApplyDotDamage(float dotDuration,float damage)
    {
        float elapsedTime = 0;
        while (elapsedTime < dotDuration) 
        {
            GetDamage(damage);
            elapsedTime += 1;
            yield return new WaitForSeconds(1);
        }
        yield break;
    }

    # region 독장판

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckFlooring(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        CheckFlooring(collision);
    }

   

    void CheckFlooring(Collider2D collision)
    {
        /*
        if (collision.CompareTag("Floor") && character.isPlayer != collision.GetComponent<Flooring>().isPlayerOwner && !isFloor)
        {
            isFloor = true;
            OnDamage( collision.GetComponent<Flooring>().tickDamage);
            DOVirtual.DelayedCall(floorTickTime, () => isFloor = false);
        }*/
    }

#endregion
}
