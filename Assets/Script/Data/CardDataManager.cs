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

public class CardDataManager : MonoBehaviour
{
   
   
   public static CardDataManager Inst;
   
   [Serializable]
   public class CardData
   {

      public Stat stat = new Stat();
      public CardData(CardUnitData.Data cardData)
      {
         stat.cardType = cardData.cardType;
         stat.cardRole = cardData.cardRole;
         stat.cardNum = cardData.cardNum;
         
         stat.bulletSize = 1;
         stat.hp = cardData.hp;
         stat.damage = cardData.damage;
         stat.speed = cardData.speed;
         stat.coolTime = cardData.coolTime;
         stat.attackCount = cardData.attackCount;
         stat.extraHitCount = cardData.extraHitCount;
         stat.criticalChance = 10;
         stat.criticalMultiplier = 1.5f;
         stat.bulletSpeed = cardData.bulletSpeed;
      }
   }

   public List<CardData> cardDatas = new List<CardData>();

   private void Awake()
   {
      if (Inst != null && Inst != this)
      {
         Destroy(gameObject);
         return;
      }
      else
      {
         Inst = this;
         DontDestroyOnLoad(gameObject);
      }
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
