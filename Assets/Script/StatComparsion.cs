using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatComparsion : MonoBehaviour
{
    [SerializeField] float upStrength, downStrength,upHealth,downHealth;

    



    public void Comparsion()
    {
        switch (TypeManager.Inst.playerCurSO.cardType)
        {
            case CardStats.CardType.A:

                break;
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
        }
     
    }
}
