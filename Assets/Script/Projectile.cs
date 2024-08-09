using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Projectile : MonoBehaviour
{
   
    public void ActiveFalse()
    {
        DOVirtual.DelayedCall(2f, () => gameObject.SetActive(false));
    }

    public void Return(Rigidbody2D rigid)
    {
        DOVirtual.DelayedCall(2f, () =>
        {
            rigid.velocity = -rigid.velocity;
            ActiveFalse();
            });
    }
}
