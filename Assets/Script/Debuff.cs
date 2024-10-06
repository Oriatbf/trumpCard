public abstract class Debuff
{
    public float duration;

    public abstract void Apply(Health health);
}

public class FireDebuff : Debuff
{
    public float dotDamage;

    public override void Apply(Health health)
    {
        health.DotDamage(duration,dotDamage);
    }
}

public class IceDebuff : Debuff
{
    public override void Apply(Health health)
    {
        health.IceAge(duration);
    }
}