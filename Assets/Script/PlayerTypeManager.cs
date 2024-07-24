using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTypeManager : MonoBehaviour
{
    public static PlayerTypeManager Inst;

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

    public void TypeChange(int num)
    {
        index = num;
        Debug.Log(index+2);
     
        player.GetComponent<SpriteRenderer>().sprite = sprites[index];

        PlayerStats.Inst.coolTime = cardSO[index].coolTime;
        PlayerStats.Inst.damage = cardSO[index].damage;
        PlayerStats.Inst.speed = cardSO[index].speed;
        PlayerStats.Inst.hp = cardSO[index].hp;
        PlayerStats.Inst.StatsApply();
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
