using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Projectile : MonoBehaviour
{
    public CharacterType ownerCharacter;
    protected Tween disableTween;
   
    public void ActiveFalse(float delay = 0f)
    {
        if (disableTween != null)
            disableTween.Kill();
        disableTween=DOVirtual.DelayedCall(delay, () =>
        {
            Debug.Log("총알 사라짐");
            ObjectPoolingManager.Inst.ReturnObjectToPool(this);
        });
    }

    public void Return(Rigidbody2D rigid)
    {
        Debug.Log("true");
        DOVirtual.DelayedCall(2f, () =>
        {
            rigid.linearVelocity = -rigid.linearVelocity;
            ActiveFalse(2f);
            });
    }

    public virtual void Init(Stat stat,Vector2 dir,CharacterType characterType)
    {
        
    }
}
