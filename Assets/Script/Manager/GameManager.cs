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
    private Character player, enemy;
    [SerializeField] MapManager mapmanager;
    [SerializeField] private Camera zoomCam;

    public void Awake()
    {
        Inst = this;
        /*
        if (Inst != this && Inst != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Inst = this;
            DontDestroyOnLoad(gameObject);
        }*/

        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Character>();

    }

    public Character GetOpponent(Character character)
    {
        if (character.characterType == CharacterType.Player)
            return enemy;
        else if (character.characterType == CharacterType.Enemy)
            return player;
        else
            return null;
        
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
        TimeManager.ChangeTimeSpeed(0.3f);
        zoomCam.transform.DOMove(new Vector3(deadCharacter.position.x, deadCharacter.position.y,-10), 0.5f);
        DOTween.To(() => zoomCam.orthographicSize, size => zoomCam.orthographicSize = size, 3, 1f);
        
        yield return new WaitForSecondsRealtime(3f);
        TimeManager.ChangeTimeSpeed(1f);
    }
    
    public IEnumerator DefectBoss(Transform deadCharacter)
    {
        yield return StartCoroutine(CameraZoom(deadCharacter.transform)); 
        SceneTransition("EndScene");
    }

    [Button]
    public IEnumerator GameEnd(bool isPlayerWin,GameObject deadCharacter)
    {
        
        yield return StartCoroutine(CameraZoom(deadCharacter.transform)); 
        if (isPlayerWin)
        {
            DataManager.Inst.Data.stage++;
            TimeManager.ChangeTimeSpeed(0f);
            RelicSelectManager.Inst.CardSelect("Map");
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
