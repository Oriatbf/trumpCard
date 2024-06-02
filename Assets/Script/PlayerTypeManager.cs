using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTypeManager : MonoBehaviour
{
    public static PlayerTypeManager Inst;

    public GameObject player,sword,gun;
    public CardStats[] cardSO;
    public Sprite[] sprites;

    public enum AttackType
    {
        Range, Melee, Magic
    }
    public AttackType attackType;

    public PlayerStats playerStats;

    private void Awake()
    {
        Inst = this;
       
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = PlayerStats.Inst;
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
                playerStats.coolTime = cardSO[i].coolTime;
                playerStats.damage = cardSO[i].damage;
                playerStats.speed = cardSO[i].speed;
                playerStats.StatsApply();
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
                        ChangeAttackType("Magic");
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
                break;
            case "Range":
                attackType = AttackType.Range;
                sword.SetActive(false);
                gun.SetActive(true);
                break;
            case "Magic":
                attackType = AttackType.Magic;
                break;
        }
    }
}
