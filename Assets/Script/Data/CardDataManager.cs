using System;
using System.Collections.Generic;
using GoogleSheet.Core.Type;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[UGS(typeof(CardType))]
public enum CardType { Melee, Range }

[UGS(typeof(CardRole))]
public enum CardRole
{
   Range,Melee,MeleeSting,Magic,ShotGun,Bow
}

public class CardDataManager : SingletonDontDestroyOnLoad<CardDataManager>
{
   [Serializable]
   public class CardData
   {

      public Stat stat = new Stat();
      public CardData(CardUnitData.Data cardData)
      {
         var _stat = this.stat;
         _stat.cardType = cardData.cardType;
         _stat.cardRole = cardData.cardRole;
         _stat.cardNum = cardData.cardNum;
         ref var basicStat = ref stat.originStatValue;
         basicStat.bulletSize = 1;
         basicStat.hp = cardData.hp;
         basicStat.damage = cardData.damage;
         basicStat.speed = cardData.speed;
         basicStat.coolTime = cardData.coolTime;
         basicStat.attackCount = cardData.attackCount;
         basicStat.extraHitCount = cardData.extraHitCount;
         basicStat.criticalChance = 10;
         basicStat.criticalMultiplier = 1.5f;
         basicStat.bulletSpeed = cardData.bulletSpeed;
      }
   }

   public List<CardData> cardDatas = new List<CardData>();

   protected override void Awake()
   {
      base.Awake();

      CardUnitData.Data.Load();
     
   }

   private void Start()
   {
      foreach (var _datas in CardUnitData.Data.DataList)
      {
         cardDatas.Add(new CardData(_datas));
      }
   }

   public CardData RandomCard()
   {
      int random = Random.Range(0, cardDatas.Count);
      return cardDatas[random];
   }
}
