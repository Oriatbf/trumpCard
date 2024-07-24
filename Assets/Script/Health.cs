using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] float curHp;
    [SerializeField] float maxHp;
    [SerializeField] Image hpBar;


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

    public void OnDamage(float damage)
    {
        curHp -= damage;
        HpBarIncrease();
        //받은 데미지의 10%초 만큼 갬블 게이지가 올라감 
        if (curHp <= 0)
        {
            curHp = maxHp;
            //사망
        }
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
}
