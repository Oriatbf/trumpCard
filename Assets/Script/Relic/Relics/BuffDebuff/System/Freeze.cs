using UnityEngine;

public class Freeze : StatusEffect
{
    public Freeze(float _duration) : base(_duration)
    {
    }

    public override void ApplyEffect(Health _health)
    {
        
    }

    public override void RemoveEffect()
    {
        throw new System.NotImplementedException();
    }
}
