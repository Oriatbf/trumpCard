using System;
using UnityEngine;
[CreateAssetMenu(fileName = "CardSO", menuName ="Scriptable CardStats/Card",order = 1)]

public class CardStats : ScriptableObject
{
    public enum CardType
    {
        Two, Three, Four, Five,Six,Seven,Eight,Nine,Ten,Jack,Queen,King,A
    }


    [Serializable]
    public struct CardData
    {
        public CardType cardType;
        public float damage;
        public float speed;
        public float hp;

    }


    public CardData[] cardData;

   


}
