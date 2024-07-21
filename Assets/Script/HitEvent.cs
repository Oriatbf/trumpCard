using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEvent : MonoBehaviour
{
    PlayerMove playerMove;
    // Start is called before the first frame update
    void Start()
    {
        playerMove= GetComponentInParent<PlayerMove>();

    }

    public void hitEvent()
    {
        playerMove.MeleeDamage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
