using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D rigid;
    public CharacterType ownerCharacter;
    protected Tween disableTween; 
    private bool isReturn = false;
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
  
    }
    
   
    public void ActiveFalseTimer(float delay = 2f,bool _return = false)
    {
        if (disableTween != null)
            disableTween.Kill();
        disableTween=DOVirtual.DelayedCall(delay, () =>
        {
            if(_return)
                Return();
            else
            {
                isReturn = false;
                ActiveFalse();
            }
                
        });
    }

    private void ActiveFalse()
    {
        ObjectPoolingManager.Inst.ReturnObjectToPool(this);
    }

    protected int RigidVelocity() => isReturn ? -1 : 1;

    protected void Return()
    {
        isReturn = true;
        ActiveFalseTimer(2f);
    }

    public virtual void Init(Stat stat,Vector2 dir,CharacterType characterType,List<StatusEffect> _debuffs)
    {
        
    }
    
    public virtual void Init(Stat stat,float damage,Vector2 dir,CharacterType characterType,List<StatusEffect> _debuffs)
    {
        
    }
}
