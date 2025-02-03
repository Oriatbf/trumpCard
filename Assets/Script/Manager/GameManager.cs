using Cinemachine;
using DG.Tweening;
using EasyTransition;
using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VInspector;


public class GameManager : Singleton<GameManager>
{
    [SerializeField] GameObject RelicSelectCanvas;
    public int stageNum;
    public CountDown countDown;
    public bool startChooseRelic = false;
    public bool bossStage;
    [HideInInspector] public bool playerDead;
    [SerializeField]private Character player, enemy;
    [SerializeField] MapManager mapmanager;
    [SerializeField] private Camera zoomCam;

    public bool gamePause = false;

    public NpcDataManager.Data enemyData;

    private void Awake()
    {
        if(!player)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        
        if(!enemy)
            enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Character>();

        enemyData = NpcDataManager.Inst.RandomEnemyForStage();
        


    }
    public bool Pause() => GameManager.Inst.gamePause;

    
    public Character GetOpponent(CharacterType characterType)
    {
        if (characterType == CharacterType.Player)
            return enemy;
        else if (characterType == CharacterType.Enemy)
            return player;
        else
            return null;
        
    }

    
    private void OnEnable()
    {
        stageNum = 0;
    }


    IEnumerator CameraZoom(Transform deadCharacter)
    {
        zoomCam.gameObject.SetActive(true);
        TimeManager.Inst.ChangeTimeSpeed(0.3f);
        zoomCam.transform.DOMove(new Vector3(deadCharacter.position.x, deadCharacter.position.y,-10), 0.5f);
        DOTween.To(() => zoomCam.orthographicSize, size => zoomCam.orthographicSize = size, 3, 1f);
        
        yield return new WaitForSecondsRealtime(3f);
        TimeManager.Inst.ChangeTimeSpeed(1f);
    }
    
    public IEnumerator DefectBoss(Transform deadCharacter)
    {
        yield return StartCoroutine(CameraZoom(deadCharacter.transform)); 
        SceneTransition("EndScene");
    }

    [Button]
    public IEnumerator GameEnd(bool isPlayerWin,Character deadCharacter)
    {
        yield return StartCoroutine(CameraZoom(deadCharacter.transform)); 
        if (isPlayerWin)
        {
            if(DataManager.Inst.bossStage)
                SceneTransition("EndScene");
            else
            {
                DataManager.Inst.Data.stage++;
                TimeManager.Inst.ChangeTimeSpeed(0f);
                RelicSelectController.Inst.CardSelect("Map");
            }
            
        }
        else
        {
            //플레이어가 졌을 때
            DataManager.Inst.ResetData();            
            SceneTransition("LobbyScene");
        }
    }

    public void SceneTransition(string sceneName)
    {
        DemoLoadScene.Inst.LoadScene(sceneName);
    }

}    
