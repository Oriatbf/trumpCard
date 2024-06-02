using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Inst;

    public GameObject player;

    public float damage, coolTime,speed;

    private void Awake()
    {
        Inst= this;
    }
    
    public void StatsApply()
    {
        PlayerMove playerMove =  player.GetComponent<PlayerMove>();
        playerMove.speed = speed;
        playerMove.coolTime= coolTime;
    }
}
