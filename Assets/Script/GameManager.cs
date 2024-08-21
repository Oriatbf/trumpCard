using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;
    public bool isGameStart,isGameEnd,mapMode;
    [SerializeField] GameObject RelicSelectCanvas;
    public int stageNum;
    public bool startChooseRelic = true;
    private Transform player, enemy;

    

    private void Awake()
    {
        if (Inst != this && Inst != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Inst = this;
            DontDestroyOnLoad(gameObject);
        }
     

       
       
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


    public void NextStage()
    {
       //enemy.GetComponent<RelicSkills>().relics = setEnemyRelic;
        stageNum++;
       
        if(stageNum != 1)
        {
            startChooseRelic= false;
            GameStart();
        }
        else
        {
            EnableRelicChoose();
        }
        SceneManager.LoadScene("StageScene");
        DOVirtual.DelayedCall(0.5f,()=> ResetStart());
    }



    public void ResetStart()
    {

        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        UIManager.Inst.GameStart();
        RelicManager.Inst.GameStart();
    }

    public void EnableRelicChoose()
    {
        RelicSelectCanvas.gameObject.SetActive(true);
    }


    public void GameEnd(bool isPlayerWin)
    {
        if (isPlayerWin)
        {
            isGameEnd= true;
            RelicManager.Inst.RandomSO();
            RelicSelectCanvas.SetActive(true);
        }
    }
}
