using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTypeManager : MonoBehaviour
{
    public static PlayerTypeManager Inst;

    public GameObject player,sword,gun,shotGun,magicWand;
    public CardStats[] cardSO;
    public Sprite[] sprites;

    public enum AttackType
    {
        Range, Melee, Magic,ShotGun
    }
    public AttackType attackType;

    public PlayerStats playerStats;

    private void Awake()
    {
        playerStats = PlayerStats.Inst;
        Inst = this;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TypeChange(int index)
    { 
        for(int i = 0;i< sprites.Length;i++)
        {
            if(index == i)
            {
                player.GetComponent<SpriteRenderer>().sprite = sprites[i];

                PlayerStats.Inst.coolTime = cardSO[i].coolTime;
                PlayerStats.Inst.damage = cardSO[i].damage;
                PlayerStats.Inst.speed = cardSO[i].speed;
                PlayerStats.Inst.StatsApply();
                switch (cardSO[i].cardType) 
                {
                    case CardStats.CardType.Two:
                        Debug.Log("2");
                        ChangeAttackType("Melee");
                        break;
                    case CardStats.CardType.Three:
                        Debug.Log("3");
                        ChangeAttackType("Range");
                        break;
                    case CardStats.CardType.Four:
                        Debug.Log("4");
                        ChangeAttackType("Melee");
                        break;
                    case CardStats.CardType.Five:
                        Debug.Log("5");
                        ChangeAttackType("Range");
                        break;
                    case CardStats.CardType.Six:
                        Debug.Log("6");
                        ChangeAttackType("Melee");
                        break;
                    case CardStats.CardType.Seven:
                        Debug.Log("7");
                        ChangeAttackType("Range");
                        break;
                    case CardStats.CardType.Eight:
                        Debug.Log("8");
                        ChangeAttackType("Melee");
                        break;
                    case CardStats.CardType.Nine:
                        Debug.Log("9");
                        ChangeAttackType("Range");
                        break;
                    case CardStats.CardType.Ten:
                        Debug.Log("10");
                        ChangeAttackType("Melee");
                        break;
                    case CardStats.CardType.Jack:
                        Debug.Log("J");
                        ChangeAttackType("ShotGun");
                        break;
                    case CardStats.CardType.King:
                        Debug.Log("K");
                        ChangeAttackType("Magic");
                        break;
                    case CardStats.CardType.Queen:
                        Debug.Log("Q");
                        ChangeAttackType("Magic");
                        break;
                    case CardStats.CardType.A:
                        Debug.Log("A");
                        ChangeAttackType("Magic");
                        break;

                }
            }
        }
    }

    void ChangeAttackType(string type)
    {
        switch(type)
        {
            case "Melee":
                attackType = AttackType.Melee;
                sword.SetActive(true);
                gun.SetActive(false);
                shotGun.SetActive(false);
                magicWand.SetActive(false);
                break;
            case "Range":
                attackType = AttackType.Range;
                sword.SetActive(false);
                gun.SetActive(true);
                shotGun.SetActive(false);
                magicWand.SetActive(false);
                break;
            case "ShotGun":
                attackType = AttackType.ShotGun;
                sword.SetActive(false);
                gun.SetActive(false);
                shotGun.SetActive(true);
                magicWand.SetActive(false);
                break;
            case "Magic":
                attackType = AttackType.Magic;
                sword.SetActive(false);
                gun.SetActive(false);
                shotGun.SetActive(false);
                magicWand.SetActive(true);
                break;
        }
    }
}
