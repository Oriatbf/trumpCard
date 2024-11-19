using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Projectile : MonoBehaviour
{
    public bool isPlayerBullet;
   
    public void ActiveFalse()
    {
        DOVirtual.DelayedCall(2f, () => gameObject.SetActive(false));
    }

    public void Return(Rigidbody2D rigid)
    {
        Debug.Log("true");
        DOVirtual.DelayedCall(2f, () =>
        {
            rigid.linearVelocity = -rigid.linearVelocity;
            ActiveFalse();
            });
    }
}
