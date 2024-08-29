using Cinemachine;
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

    Camera zoomCam;

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
        zoomCam = GameObject.FindGameObjectWithTag("CinemaCam").GetComponent<Camera>();
        zoomCam.gameObject.SetActive(false);

    }

    public void EnableRelicChoose(bool startRelic)
    {
        RelicSelectCanvas.gameObject.SetActive(startRelic);
    }

    private void OnDisable()
    {
        Debug.Log("diaablse");
    }

    [Button]
    public void GameEnd(bool isPlayerWin,GameObject deadCharacter)
    {
        zoomCam.gameObject.SetActive(true);
        isGameStart = false;
        zoomCam.transform.position= new Vector3( deadCharacter.transform.position.x, deadCharacter.transform.position.y,-10);
        DOTween.To(() => zoomCam.orthographicSize, size => zoomCam.orthographicSize = size, 3, 1f);
        Time.timeScale = 0.3f;
        Time.fixedDeltaTime = 1 / Time.timeScale * 0.02f;

        DOVirtual.DelayedCall(3f, () =>
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f;
         //   deadCharacter.gameObject.SetActive(false);
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
                UIManager.Inst.GoldRelicReset();
                SceneTransition("LobbyScene");
                Destroy(gameObject, 2f);

            }
        });

        
    }

    public void SceneTransition(string sceneName)
    {
        if (sceneName == "LobbyScene") isLobby = true;
        else isLobby = false;
        DemoLoadScene.Inst.LoadScene(sceneName);
    }

}    
