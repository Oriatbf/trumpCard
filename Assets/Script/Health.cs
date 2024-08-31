using DamageNumbersPro;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class Health : MonoBehaviour
{

    [Tab("Hp")]
    //Hp Value
    public float curHp;
    public float maxHp;
    [SerializeField] DamageNumber numberPrefab;
    [SerializeField] RectTransform rectParent;
    [SerializeField] float floorTickTime =1;
    [SerializeField] Image hpBar;
    [SerializeField] Material whiteMaterial,defaultMaterial;
    [SerializeField] SpriteRenderer spr;
    [HideInInspector] public bool autoHeal;
    [HideInInspector] public float autoHealSpeed;
    [HideInInspector] public bool isFloor;
    bool isInv = false;
    bool death = false;

    [Tab("Debuff")]
    public bool inFireDebuff,inFreezeDebuff;
    public float fireDotDam,freezeTime;

    //GambleGauge Value
    GambleGauge gambleGauge;
    private float hittedValue = 2f,attackValue = 1f; //hitted : 맞았을 때 , attack : 공격했을 때 늘어날 값
    Character character;
    EnemyMove enemyMove;

    private void Start()
    {
        gambleGauge=GetComponent<GambleGauge>();
        character= GetComponent<Character>();
        hpBar.fillAmount = 1;
        if(!character.isPlayer)enemyMove=GetComponent<EnemyMove>();
    }

    private void Update()
    {
        if(autoHeal)AutoHeal();
        FireDebuff();
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

    public void OnDamage(float damage)
    {
        if (!isInv )
        {
            spr.material = whiteMaterial;
            DOVirtual.DelayedCall(0.1f, () => spr.material = defaultMaterial);
            curHp -= damage;
            HpBarIncrease();
            if (!character.isPlayer)
            {
                if (curHp <= maxHp / 2 && enemyMove.enemyCharacter == EnemyMove.EnemyCharacter.Boss) character.extraAttackRatio = 2;

            }
            
            IncreaseGambleGauge(true,damage); // 피격 게이지 올라가기
            EnemyUp(damage);
            
            if(damage >=0)
                numberPrefab.SpawnGUI(rectParent,transform.position,damage);


            if (curHp <= 0 && !death)
            {
                death = true;
                if (!character.isPlayer)
                {
                    UIManager.Inst.GoldCount(character.goldValue);
                    if(enemyMove.enemyCharacter == EnemyMove.EnemyCharacter.Boss)
                    {
                        GameManager.Inst.DefectBoss(gameObject);
                    }
                    else
                    {
                        GameManager.Inst.GameEnd(true, gameObject);
                    }
                  
                }
                else
                {
                    GameManager.Inst.GameEnd(false,gameObject);

                }
              
            }
        }
       
    }

    void FreezeDebuff()
    {
        if (inFreezeDebuff)
        {
            freezeTime -= Time.deltaTime;
            if (freezeTime > 0)
            {

            }
            else inFreezeDebuff= false;
        }
    }

    void FireDebuff()
    {
        if (inFireDebuff)
        {
            OnDamage(fireDotDam*Time.deltaTime);
        }
    }

    void EnemyUp(float damage)
    {
        Health op_health =  character.opponent.GetComponent<Health>();
        Character op_character= character.opponent.GetComponent<Character>();
        op_health.IncreaseGambleGauge(false,damage); // 적 캐릭터가 공격 시 올라가는 갬블게이지
        if (op_character.characterSO.relicInfor.bloodSucking)
        {
            op_health.OnHeal(damage * 0.5f); // 적 캐릭터가 피흡 보유시 힐   
            Debug.Log("피흡");
        }
    }

    public void IncreaseGambleGauge(bool isHitted,float damage)
    {
        if (isHitted) gambleGauge.IncreaseGambleGauge(damage * hittedValue);
        else gambleGauge.IncreaseGambleGauge(damage*attackValue);
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
        if (collision.CompareTag("Floor") && character.isPlayer != collision.GetComponent<Flooring>().isPlayerOwner && !isFloor)
        {
            isFloor = true;
            OnDamage( collision.GetComponent<Flooring>().tickDamage);
            DOVirtual.DelayedCall(floorTickTime, () => isFloor = false);
        }
    }
}
