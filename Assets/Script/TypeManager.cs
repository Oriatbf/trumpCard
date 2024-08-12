using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeManager : MonoBehaviour
{
    public static TypeManager Inst;

    private GameObject player, enemy;
    public CardStats[] cardSO;

    public enum AttackType
    {
        Range, Melee, Magic,ShotGun
    }
    public AttackType attackType;


    private void Awake()
    {
       
        Inst = this;
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");

    }
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    public void TypeChange(int num, Transform character, bool isPlayer,CardStats characterSO)
    {

        characterSO.infor = cardSO[num].infor;
       
       
        if (character.TryGetComponent(out SpriteRenderer sprite))
            sprite.sprite = isPlayer ? cardSO[num].infor.playerCardImage : cardSO[num].infor.enemyCardImage;
        ChangeAttackType(isPlayer,num);
     
        
    }

    void ChangeAttackType(bool isPlayer,int num)
    {
        GameObject curCharacter;
        curCharacter = isPlayer ? player : enemy;
        Character character = curCharacter.GetComponent<Character>();  
        character.SetWeapon(num);   
    }
}
