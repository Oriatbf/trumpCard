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
    
    private void OnEnable()
    {
        stageNum = 0;
    }


    public void FightStage()
    {

        
        

        if (stageNum != 1)
        {
            startChooseRelic = false;

        }
        else
        {
            startChooseRelic = true;
        }
        SceneTransition("StageScene");
    }

    public void StageNext()
    {
        stageNum++;
    }


    void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {
    /*
        if (!mapMode && !playerDead && !isLobby)
        {
            GameStart();
            ResetStart();
          //  EnableRelicChoose(startChooseRelic);
        }*/

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
           // UIManager.Inst.GoldRelicReset();
            Destroy(GameManager.Inst.gameObject);
        }

        
        if (newScene.name == "LobbyScene")
        {
           // UIManager.Inst.GoldRelicReset();
            Destroy(GameManager.Inst.gameObject);
        }


        if(newScene.name == "EndScene")
        {
            //UIManager.Inst.EndingScene();
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
   
    IEnumerator CameraZoom(Transform deadCharacter)
    {
        zoomCam.gameObject.SetActive(true);
        zoomCam.transform.DOMove(new Vector3(deadCharacter.position.x, deadCharacter.position.y), 0.35f);
        DOTween.To(() => zoomCam.orthographicSize, size => zoomCam.orthographicSize = size, 3, 1f);
        TimeManager.ChangeTimeSpeed(0.3f);
        yield return new WaitForSeconds(3f);
    }
    
    public IEnumerator DefectBoss(Transform deadCharacter)
    {
        yield return StartCoroutine(CameraZoom(deadCharacter.transform)); 
        TimeManager.ChangeTimeSpeed(1);
        SceneTransition("EndScene");
    }

    [Button]
    public IEnumerator GameEnd(bool isPlayerWin,GameObject deadCharacter)
    {
        yield return StartCoroutine(CameraZoom(deadCharacter.transform)); 
        TimeManager.ChangeTimeSpeed(1);
        if (isPlayerWin)
        {
            RelicManager.Inst.SetRelic();
            RelicManager.Inst.playerRelic = player.GetComponent<RelicSkills>().relics; //렐릭 매니저에 플레이어 유물 저장
            RelicSelectCanvas.SetActive(true);
        }
        else
        {
            SceneTransition("LobbyScene");
        }
    }

    public void SceneTransition(string sceneName)
    {
        DemoLoadScene.Inst.LoadScene(sceneName);
    }

}    
