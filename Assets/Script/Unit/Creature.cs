using System;
using UnityEngine;

public class Creature : Unit
{
    [SerializeField] protected Stat.StatsValue statsValue = new Stat.StatsValue();
    protected SpriteRenderer spr;
    public Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
        spr = GetComponent<SpriteRenderer>();
    }

    public virtual void Init(Character character)
    {
        characterType = character.characterType;
    }

    public void UpgradeStat(float multiplier)
    {
        statsValue = statsValue * multiplier;
    }
}
