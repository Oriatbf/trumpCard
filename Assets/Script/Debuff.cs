public abstract class Debuff
{
    public float duration;

    public abstract void Apply(UnitHealth unitHealth);
}

public class FireDebuff : Debuff
{
    public float dotDamage;

    public override void Apply(UnitHealth unitHealth)
    {
        unitHealth.DotDamage(duration,dotDamage);
    }
}

public class IceDebuff : Debuff
{
    public override void Apply(UnitHealth unitHealth)
    {
       // unitHealth.IceAge(duration);
    }
}