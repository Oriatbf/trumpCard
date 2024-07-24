using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Inst;

    public GameObject player;

    public float damage, coolTime,speed,hp;

    private void Awake()
    {
        Inst= this;
    }
    
    public void StatsApply()
    {
        Health health = player.GetComponent<Health>();
        PlayerMove playerMove =  player.GetComponent<PlayerMove>();
        playerMove.speed = speed;
        playerMove.coolTime= coolTime;
        health.SetHp(hp);

    }
}
