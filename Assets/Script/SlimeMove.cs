using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SlimeMove : Creature
{
    private Character opponent;
    [SerializeField] float floorDestoryTime;
    [SerializeField] GameObject floor;
    bool isPlayer;



    

    public override void Init(Character character)
    {
        base.Init(character);
        opponent = GameManager.Inst.GetOpponent(character);
        health.ResetHp(statsValue.hp,statsValue.hp);
    }

    // Update is called once per frame
    void Update()
    {
        if (opponent != null)
        {
            if (opponent.transform.position.x < transform.position.x) spr.flipX = false;
            else spr.flipX = true;
            Vector3 dir = (opponent.transform.position - transform.position).normalized;
            transform.eulerAngles = dir;
            transform.Translate(dir * statsValue.speed * Time.deltaTime);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Character reachedChar))
        {
            if (reachedChar.characterType != characterType)
            {
                reachedChar.unitHealth.GetDamage(statsValue.damage);
                //GameObject floorObj = Instantiate(floor, transform.position, Quaternion.identity);
               // Destroy(floorObj, floorDestoryTime);
                Destroy(gameObject);
            }
        }
        
    }
}
