using Cinemachine;
using DG.Tweening;
using EasyTransition;
using Map;
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
    public bool bossStage;
    [HideInInspector] public bool playerDead;
    private Transform player, enemy;
    [SerializeField] MapManager mapmanager;
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
      
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void GameStart()
    {
       DOVirtual.DelayedCall(1.5f,()=> countDown.CountStart());
    }


    public void FightStage()
    {


        RelicManager.Inst.enemyRelic = MapSetEnemtRelc.Inst.enemyRelicSOs[stageNum];
        

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

    public void StageNext()
    {
        stageNum++;
    }


    void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {

        if (!mapMode && !playerDead && !isLobby)
        {
            GameStart();
            ResetStart();
          //  EnableRelicChoose(startChooseRelic);
        }

        if(newScene.name == "LobbyScene")
        {
          
        }
        else
        {
            if(SceneManager.GetActiveScene().buildIndex == 0)
            {
                UIManager.Inst.GoldRelicReset();
                Destroy(gameObject);
            }
        }

        if(newScene.name == "EndScene")
        {
            UIManager.Inst.EndingScene();
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

    public void DefectBoss(GameObject deadCharacter)
    {
        CameraZoom(deadCharacter);
        isLobby = true;
        DOVirtual.DelayedCall(3f, () =>
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f;
            SceneTransition("EndScene");
        });
    }
    void CameraZoom(GameObject deadCharacter)
    {
        zoomCam.gameObject.SetActive(true);
        isGameStart = false;
        zoomCam.transform.position = new Vector3(deadCharacter.transform.position.x, deadCharacter.transform.position.y, -10);
        DOTween.To(() => zoomCam.orthographicSize, size => zoomCam.orthographicSize = size, 3, 1f);
        Time.timeScale = 0.3f;
        Time.fixedDeltaTime = 1 / Time.timeScale * 0.02f;
    }

    [Button]
    public void GameEnd(bool isPlayerWin,GameObject deadCharacter)
    {
        isGameEnd = true;
        CameraZoom(deadCharacter);

        DOVirtual.DelayedCall(3f, () =>
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f;
         //   deadCharacter.gameObject.SetActive(false);
            if (isPlayerWin)
            {

               
                RelicManager.Inst.SetRelic();
                RelicManager.Inst.playerRelic = player.GetComponent<RelicSkills>().relics; //렐릭 매니저에 플레이어 유물 저장
                RelicSelectCanvas.SetActive(true);
            }
            else
            {
                isLobby = true;
                playerDead = true;
                SceneTransition("LobbyScene");

            }
        });

        
    }

    public void SceneTransition(string sceneName)
    {
        if (sceneName == "LobbyScene" || sceneName =="EndScene") isLobby = true;
        else isLobby = false;
        DemoLoadScene.Inst.LoadScene(sceneName);
    }

}    
