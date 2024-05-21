using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTypeManager : MonoBehaviour
{
    public static PlayerTypeManager Inst;

    private GameObject player;
    public CardStats cardSO;
    public Sprite[] sprites;

    public enum CardType
    {
        Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, A
    }

    public enum PlayerType
    {
        melee,range,magic
    }

    public PlayerType playerType;
    public CardType cardType;

    private void Awake()
    {
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
                switch(cardSO.cardData[i].cardType) 
                {
                    case CardStats.CardType.Two:
                        break;
                    case CardStats.CardType.Three: 
                        break;
                    case CardStats.CardType.Four: 
                        break;
                    case CardStats.CardType.Five: 
                        break;
                    case CardStats.CardType.Six:
                        break;
                    case CardStats.CardType.Seven:
                        break;
                    case CardStats.CardType.Eight:
                        break;

                }
            }
        }
    }
}
