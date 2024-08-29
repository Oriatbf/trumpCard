using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "CardSO", menuName = "Scriptable SO/Card", order = 1)]


public class CardStats : ScriptableObject
{
    public enum CardType
    {
        Two, Three, Four, Five,Six,Seven,Eight,Nine,Ten,Jack,Queen,King,A
    }
    public enum Role { Melee, Range }
    public enum AttackType
    {
        Range,Melee,MeleeSting,Magic,ShotGun,Bow
    }

    public enum Character
    {
        card,playerAble
    }

    public Character character;
    
   
    [Serializable]
    public struct Information
    {
        public Sprite playerCardImage, enemyCardImage;
        public CardType cardType;
        public Role role;
        public AttackType attackType;
        public int bulletTypeIndex;
        public float damage;
        public float coolTime;
        public float speed;
        public float hp;
        public int bulletCount;
        public int attackCount;
        public int cardNum;
        public bool projectileTurnback;
        [ShowIfEnum("attackType", (int)AttackType.Bow)] public float maxCharge;
        [ShowIfEnum("attackType", (int)AttackType.Bow)] public float maxChargeDam;


    }
    public Information infor;

    [Serializable]
    public struct RelicInfor
    {
        public Transform characterTrans;
        public Health characterHealth;
        public float remnantHealth;
        public float size;
        public bool isFlooring;
        public float floorTickDamage;
        public int criticalChance;
        public float criticalDamage;


    }

    public List<int> bloodSuckingInchant = new List<int>();
    [ShowIfEnum("character", (int)Character.playerAble)] public RelicInfor relicInfor;
   












}
