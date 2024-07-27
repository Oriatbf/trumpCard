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
    
    public void StatsApply(float speed,float hp,float coolTime)
    {
        Health health = null;
        if (player.TryGetComponent<Health>(out Health _health))
            health = _health;
        PlayerMove playerMove =  player.GetComponent<PlayerMove>();
        playerMove.speed = speed;
        playerMove.coolTime= coolTime;
        
        if(health != null)
           health.SetHp(hp);

    }
}
