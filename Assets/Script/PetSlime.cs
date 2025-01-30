using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PetSlime : Creature
{
   
    [SerializeField] float floorDestoryTime;
    [SerializeField] GameObject floor;


    protected override void Start()
    {
        base.Start();
    }

    public override void Init(Character character)
    {
        base.Init(character);
    }

    // Update is called once per frame
    void Update()
    {
        if (opponent != null)
        {
            base.Update();

            
            pathfindAI.PathfindClose();
        }
        
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Health _health))
        {
            if (_health.characterType != health.characterType)
            {
                _health.GetDamage(stat.FinalValue().damage);
                //GameObject floorObj = Instantiate(floor, transform.position, Quaternion.identity);
               // Destroy(floorObj, floorDestoryTime);
                Destroy(gameObject);
            }
        }
        
    }
}
