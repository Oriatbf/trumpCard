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
        float remnantHp = characterSO.relicInfor.characterHealth.maxHp - characterSO.relicInfor.characterHealth.curHp;
        characterSO.infor = cardSO[num].infor;
        characterSO.relicInfor.remnantHealth = cardSO[num].infor.hp - remnantHp;


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
