using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeManager : MonoBehaviour
{
    public static TypeManager Inst;

    private GameObject player, enemy;
    public CardStats[] cardSO;
    public CardStats playerCurSO,enemyCurSO;
    public Sprite[] sprites;

    public enum AttackType
    {
        Range, Melee, Magic,ShotGun
    }
    public AttackType attackType;


    private void Awake()
    {
       
        Inst = this;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        
    }

    public void TypeChange(int num,Transform character,bool isPlayer)
    {
        float coolTime, speed, hp, damage;
        if (character.TryGetComponent(out SpriteRenderer sprite))
            sprite.sprite = isPlayer ? cardSO[num].playerCardImage : cardSO[num].enemyCardImage;

        coolTime = cardSO[num].coolTime;
        damage = cardSO[num].damage;
        speed = cardSO[num].speed;
        hp = cardSO[num].hp;

        CharacterStats.Inst.StatsApply(speed,hp,coolTime,damage,isPlayer);
        switch (isPlayer)
        {
            case true:
                playerCurSO = cardSO[num];
                break;
            case false:
                enemyCurSO = cardSO[num];
                break;
        }
       
        ChangeAttackType(isPlayer,num);
     
        
    }

    void ChangeAttackType(bool isPlayer,int num)
    {
        GameObject curCharacter;
        curCharacter = isPlayer ? player : enemy;
        Character character = curCharacter.GetComponent<Character>();

        switch (cardSO[num].attackType)
        {
            case CardStats.AttackType.Melee:
                character.SetWeapon(0);
                break;
            case CardStats.AttackType.Range:
                character.SetWeapon(1);
                break;
            case CardStats.AttackType.ShotGun:
                character.SetWeapon(2);
                break;
            case CardStats.AttackType.Magic:
                character.SetWeapon(3);
                break;
        }
    }
}
