using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;
    public bool isGameStart,isGameEnd;
    private Transform player, enemy;

    private void Awake()
    {
        Inst = this;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameStart()
    {
        isGameStart = true;
        player.GetComponent<Character>().StartRelicSkill();
        enemy.GetComponent<Character>().StartRelicSkill();
    }
}
