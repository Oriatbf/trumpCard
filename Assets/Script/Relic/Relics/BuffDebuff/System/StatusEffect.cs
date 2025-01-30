using System;
using UnityEngine;

[Serializable]
public abstract class StatusEffect 
{
    protected float duration;
    protected Health target;

    public StatusEffect(float _duration)
    {
        duration = _duration;
    }

    public abstract void ApplyEffect(Health _health);
    public abstract void RemoveEffect();
}
