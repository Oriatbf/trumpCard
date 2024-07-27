using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeManager : MonoBehaviour
{
    public static TypeManager Inst;

    public GameObject player,sword,gun,shotGun,magicWand;
    public CardStats[] cardSO;
    public CardStats curSO;
    public Sprite[] sprites;
    public int index;

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
        
    }

    public void TypeChange(int num,Transform character,bool isPlayer)
    {
        index = num;
        float coolTime, speed, hp, damage;
        if (character.TryGetComponent<SpriteRenderer>(out SpriteRenderer sprite))
            sprite.sprite = isPlayer ? cardSO[index].playerCardImage : cardSO[index].enemyCardImage;

        coolTime = cardSO[index].coolTime;
        PlayerStats.Inst.damage = cardSO[index].damage;
        speed = cardSO[index].speed;
        hp = cardSO[index].hp;
        PlayerStats.Inst.StatsApply(speed,hp,coolTime);
        curSO = cardSO[index];
        ChangeAttackType();
     
        
    }

    void ChangeAttackType()
    {
        sword.SetActive(false);
        gun.SetActive(false);
        shotGun.SetActive(false);
        magicWand.SetActive(false);
        switch (cardSO[index].attackType)
        {
            case CardStats.AttackType.Melee:  
                sword.SetActive(true);
                break;
            case CardStats.AttackType.Range:
                gun.SetActive(true);
                break;
            case CardStats.AttackType.ShotGun:
                shotGun.SetActive(true);
                break;
            case CardStats.AttackType.Magic:
                magicWand.SetActive(true);
                break;
        }
    }
}
