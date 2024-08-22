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
    bool playerDead;
    private Transform player, enemy;

    

    public  void Awake()
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

        SceneManager.activeSceneChanged += OnActiveSceneChanged;


    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        RelicManager.Inst.GameStart();
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
        Debug.Log("next");
       //enemy.GetComponent<RelicSkills>().relics = setEnemyRelic;
        stageNum++;
       
        if(stageNum != 1)
        {
            startChooseRelic= false;

        }
        else
        {
            EnableRelicChoose();
        }
        mapMode = false;
        SceneManager.LoadScene("StageScene");
        Debug.Log("startScene");
       // ResetStart();
    }
   

    void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {
        if(!mapMode && !playerDead)ResetStart();
        Debug.Log("Active scene changed from " + previousScene.name + " to " + newScene.name);
        // 씬이 변경되었을 때 하고 싶은 작업을 여기서 처리
    }


    public void ResetStart()
    {
        Debug.Log("Restart");
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        UIManager.Inst.GameStart();
      
    }

    public void EnableRelicChoose()
    {
        RelicSelectCanvas.gameObject.SetActive(true);
    }


    public void GameEnd(bool isPlayerWin)
    {
        Time.timeScale = 0;
        if (isPlayerWin)
        {
            isGameEnd= true;
            RelicManager.Inst.GameStart();
            RelicManager.Inst.playerRelic =  player.GetComponent<RelicSkills>().relics;
            RelicSelectCanvas.SetActive(true);
        }
        else
        {
            playerDead = true;
            SceneManager.LoadScene("LobbyScene");
            Destroy(gameObject);
           
        }
    }
}
