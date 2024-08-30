using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;



public class RelicSO : ScriptableObject
{
    public string relicName;
    public Sprite relicIcon;
    public string relicInfor;
    public enum Rarity {Common,Rare,Epic };
    public Rarity rarity;

    

    public virtual void Active(CardStats so, BasicAbility.RelicType relicType)
    {

    }

    public virtual void Active(CardStats so, SpecialAbility.RelicType relicType)
    {
        
    }

    public virtual void Active(CardStats so, InChantRelic.RelicType relicType)
    {

    }
}
