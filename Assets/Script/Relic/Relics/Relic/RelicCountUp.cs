using Unity.VisualScripting;
using UnityEngine;

public class RelicCountUp : RelicBase
{
    public override void Excute()
    {
        base.Excute();
        DataManager.Inst.Data.cardCount +=  (int)value;
    }
    
}
