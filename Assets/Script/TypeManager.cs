using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeManager : MonoBehaviour
{
    public static TypeManager Inst;

    public CardStats[] cardSO;



    private void Awake()
    {
        if (Inst != this && Inst != null)
        {
            return;
        }
        else
        {
            Inst = this;
        }


    }
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    public void TypeChange(int num, Transform character, bool isPlayer,CardStats characterSO)
    {
        float remnantHp = characterSO.relicInfor.characterHealth.maxHp - characterSO.relicInfor.characterHealth.curHp;  //바뀌기 전 최대체력 - 현재체력 = 닳은 체력 
        characterSO.infor = cardSO[num].infor;
        float isHpZero = cardSO[num].infor.hp - remnantHp;
       
        characterSO.relicInfor.remnantHealth = isHpZero>0? cardSO[num].infor.hp - remnantHp:1;  //바뀐 최대체력 - 닳은 체력 



        if (character.TryGetComponent(out SpriteRenderer sprite))
            sprite.sprite = isPlayer ? cardSO[num].infor.playerCardImage : cardSO[num].infor.enemyCardImage;
        ChangeAttackType(character,num);      
    }

    void ChangeAttackType(Transform character,int num)
    {

        Character ch = character.GetComponent<Character>();  
        ch.SetWeapon(num);   
    }
}
