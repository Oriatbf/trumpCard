using System;
using UnityEngine;
[CreateAssetMenu(fileName = "CardSO", menuName ="Scriptable CardStats/Card",order = 1)]

public class CardStats : ScriptableObject
{
    public enum CardType
    {
        Two, Three, Four, Five,Six,Seven,Eight,Nine,Ten,Jack,Queen,King,A
    }

    public enum AttackType
    {
        Range,Melee,Magic,ShotGun
    }

    public CardType cardType;
    public AttackType attackType;
    public float damage;
    public float coolTime;
    public float speed;
    public float hp;
    public string cardName;

    


   

   


}
