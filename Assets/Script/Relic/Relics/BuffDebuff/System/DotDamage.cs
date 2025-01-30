using System;
using System.Collections;
using UnityEngine;
    
[Serializable]
public class DotDamage : StatusEffect
{
    private float damage;
    private Coroutine cor;
    public DotDamage(float _duration,float _damage) : base(_duration)
    {
        damage = _damage;
    }

    public override void ApplyEffect(Health _health)
    {
        if(cor != null)
            GameManager.Inst.StopCoroutine(cor);
        target = _health;
        cor = GameManager.Inst.StartCoroutine(DamageOverTime());
    }

    public override void RemoveEffect()
    {
        if(cor != null)
            GameManager.Inst.StopCoroutine(cor);
    }

    private IEnumerator DamageOverTime()
    {
        float _duration = duration;
        while (_duration > 0)
        {
            // 피해를 입히는 부분
            DamageToTarget();

            // 주기적으로 피해를 입히는 간격(예: 1초)을 기다립니다.
            yield return new WaitForSeconds(1f);
            
            _duration -= 1f;
        }
        RemoveEffect();
    }

    private void DamageToTarget()
    {
        target.GetDamage(damage);
    }
}
