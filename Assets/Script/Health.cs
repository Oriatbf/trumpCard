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
    [SerializeField] DamageNumber numberPrefab;
    [SerializeField] private RectTransform rectParent;
    [SerializeField] Image hpBar;
    [SerializeField] Material whiteMaterial,defaultMaterial;
    [SerializeField] SpriteRenderer spr;
    [HideInInspector] public bool autoHeal;
    [HideInInspector] public float autoHealSpeed;
    [SerializeField]bool isInv = false;

    public Action OnDamage,OnHeal;

    //GambleGauge Value
    GambleGauge gambleGauge;
    private float hittedValue = 5f; //hitted : 맞았을 때 
    Character character;
    EnemyMove enemyMove;
    Coroutine freezeDebuff;
    private void Start()
    {
        gambleGauge=GetComponent<GambleGauge>();
        character= GetComponent<Character>();
        hpBar.fillAmount = 1;
        OnDamage += () => gambleGauge.IncreaseGambleGauge(hittedValue);
        OnDamage += () => HpBarIncrease();
        OnHeal += () => HpBarIncrease();


        //  if(!character.isPlayer)enemyMove=GetComponent<EnemyMove>();
    }

    private void Update()
    {
        if(autoHeal)AutoHeal();
    }
    public void SetHp(float remnantHp)
    {
        curHp = remnantHp;
        HpBarIncrease();
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
                character.opponent.GetComponent<Health>().isInv = true;
                /*
                if (!character.isPlayer)
                {
                    if (enemyMove.enemyCharacter == EnemyMove.EnemyCharacter.Boss)
                        StartCoroutine(GameManager.Inst.DefectBoss(transform));
                    else StartCoroutine( GameManager.Inst.GameEnd(true, gameObject));
                }
                else StartCoroutine(GameManager.Inst.GameEnd(false, gameObject));*/
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

    public void HpBarIncrease() => hpBar.fillAmount = curHp / maxHp;
    public void InvTime(float time)
    {
        isInv = true;
        DOVirtual.DelayedCall(time, () => isInv = false);
    }

    public void DotDamage(float time,float damage)
    {
        StartCoroutine( ApplyDotDamage(time, damage));
    }
    public void IceAge(float time)
    {
        if (freezeDebuff != null) StopCoroutine(freezeDebuff);

        freezeDebuff =  StartCoroutine(ApplyIce(time));
    }

    IEnumerator ApplyIce(float iceDuration)
    {
        float elapsedTime = 0;
       // character.moveBlock = true;
        while (elapsedTime < iceDuration)
        {           
            elapsedTime += Time.deltaTime;
            yield return null;
        }
      //  character.moveBlock = false;
        freezeDebuff = null;
        yield break;
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
