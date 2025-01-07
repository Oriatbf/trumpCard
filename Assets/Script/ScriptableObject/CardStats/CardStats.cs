using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CardSO", menuName = "Scriptable SO/Card", order = 1)]


public class CardStats : ScriptableObject
{

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
        public Role role;
        public AttackType attackType;
        public int bulletTypeIndex;
        public float damage;
        public float coolTime;
        public float speed;
        public float hp;
        public int extraHitCount;
        public int attackCount;
        public int cardNum;
        public bool projectileTurnback;
        [ShowIfEnum("attackType", (int)AttackType.Bow)] public float maxCharge;
        [ShowIfEnum("attackType", (int)AttackType.Bow)] public float plusMaxChargeDam;


    }
    public Information infor;

    [Serializable]
    public struct RelicInfor
    {
        public Transform characterTrans;
        [FormerlySerializedAs("characterHealth")] public UnitHealth characterUnitHealth;
        public float remnantHealth;
        public float relicPlusHealth;
        public float size;
        public bool isFlooring;
        public float floorTickDamage;
        public int criticalChance;
        public float criticalDamage;
        public bool isSlash;
        public bool bloodSucking;
      
    }

    
    public List<int> bloodInchant,fireInchant,iceInchant = new List<int>();
    public List<Debuff> debuffs = new List<Debuff>();

    [ShowIfEnum("character", (int)Character.playerAble)] public RelicInfor relicInfor;

    public void ResetRelicInfor()
    {
        relicInfor.characterTrans = null;
        relicInfor.characterUnitHealth = null;
        relicInfor.remnantHealth = 0;
        relicInfor.relicPlusHealth= 0;
        relicInfor.size = 1;
        relicInfor.isFlooring = false;
        relicInfor.floorTickDamage= 0;
        relicInfor.criticalChance = 7;
        relicInfor.criticalDamage = 1.3f;
        relicInfor.isSlash = false;
        relicInfor.bloodSucking = false;
    }
   
    public void ClearDebuffList()
    {
        bloodInchant.Clear();
        fireInchant.Clear();
        iceInchant.Clear();
        debuffs.Clear();
    }

}
