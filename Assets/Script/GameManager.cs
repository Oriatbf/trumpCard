using DG.Tweening;
using EasyTransition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VInspector;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;
    public bool isGameStart, isGameEnd, mapMode, isLobby;
    [SerializeField] GameObject RelicSelectCanvas;
    public int stageNum;
    public CountDown countDown;
    public bool startChooseRelic = false;
    [HideInInspector] public bool playerDead;
    private Transform player, enemy;



    public void Awake()
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
        DOVirtual.DelayedCall(1.5f,()=> countDown.CountStart());
        

        //처음에 렐릭 선택하고 바로 적용시킬 때
     //   player.GetComponent<Character>().StartRelicSkill();
      //  enemy.GetComponent<Character>().StartRelicSkill();
    }


    public void NextStage()
    {

        RelicManager.Inst.enemyRelic = MapSetEnemtRelc.Inst.enemyRelicSOs[stageNum];
        stageNum++;

        if (stageNum != 1)
        {
            startChooseRelic = false;

        }
        else
        {
            startChooseRelic = true;
        }
        mapMode = false;
        SceneTransition("StageScene");
    }


    void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {


        if (!mapMode && !playerDead && !isLobby)
        {
            GameStart();
            ResetStart();
          //  EnableRelicChoose(startChooseRelic);
        }

        Debug.Log("Active scene changed from " + previousScene.name + " to " + newScene.name);
        // 씬이 변경되었을 때 하고 싶은 작업을 여기서 처리
    }


    public void ResetStart()
    {
        Debug.Log("Restart");
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;

    }

    public void EnableRelicChoose(bool startRelic)
    {
        RelicSelectCanvas.gameObject.SetActive(startRelic);
    }


    public void GameEnd(bool isPlayerWin)
    {
        isGameStart = false;
        if (isPlayerWin)
        {
            
            isGameEnd = true;
            RelicManager.Inst.GameStart();
            RelicManager.Inst.playerRelic = player.GetComponent<RelicSkills>().relics; //렐릭 매니저에 플레이어 유물 저장
            RelicSelectCanvas.SetActive(true);
        }
        else
        {
            isLobby = true;
            playerDead = true;
            SceneTransition("LobbyScene");
            Destroy(gameObject,2f);

        }
    }

    public void SceneTransition(string sceneName)
    {
        DemoLoadScene.Inst.LoadScene(sceneName);
    }

}    
