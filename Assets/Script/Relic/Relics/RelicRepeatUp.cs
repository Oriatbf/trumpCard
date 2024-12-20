using UnityEngine;

public class RelicRepeatUp : RelicBase
{
    public override void Excute()
    {
        base.Excute();
        Debug.Log("리피트업");
        DataManager.Inst.Data.cardRepeat += (int)value;
    }
}
