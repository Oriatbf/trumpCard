using System;
using DamageNumbersPro;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using VInspector;
using VInspector.Libs;

public class UnitHealth : Health
{
    
    [SerializeField] private RectTransform rectParent;

    //GambleGauge Value
    GambleGauge gambleGauge;
    private float hittedValue = 5f; 
    Character character;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        gambleGauge=GetComponent<GambleGauge>();
        character= GetComponent<Character>();
        base.Start();
    }

    protected override void SetAction()
    {
        base.SetAction();
        OnDamage += () => gambleGauge.IncreaseGambleGauge(hittedValue);
        OnAttack += () => gambleGauge.IncreaseGambleGauge(hittedValue / 2);
        OnDamage += () => GameManager.Inst.GetOpponent(characterType).unitHealth.OnAttack?.Invoke();
    }

    

    public override void GetDamage(float damage)
    {
        if (!isInv )
        {
            spr.material = whiteMaterial;
            DOVirtual.DelayedCall(0.1f, () => spr.material = defaultMaterial);
            curHp -= damage;
            if(damage >=0)
                UIManager.Inst.DamageUI(rectParent,transform,damage);

            OnDamage?.Invoke();
            if (curHp <= 0)
            {
                isInv = true;
                character.opponent.GetComponent<UnitHealth>().isInv = true;
                if (characterType == CharacterType.Player)
                {
                    StartCoroutine( GameManager.Inst.GameEnd(false, character));
                }
                else if (characterType == CharacterType.Enemy)
                {
                    StartCoroutine(GameManager.Inst.GameEnd(true, character));
                     TopUIController.Inst.GetGold(character.goldValue); 
                }
                return;
            }

            

        }
       
    }


    public override void OnRecorvery(float healAmount)
    {
        base.OnRecorvery(healAmount);
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
