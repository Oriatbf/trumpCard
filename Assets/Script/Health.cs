using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float curHp;
    [SerializeField] float maxHp;

    public void OnDamage(float damage)
    {
        curHp-= damage;
        if (curHp <= 0)
        {
            //사망
        }
    }

    public void OnHeal(float healAmount)
    {
        curHp += healAmount;
        if(curHp > maxHp)
            curHp = maxHp;
    }
}
