using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class PlayableNpcManager : MonoBehaviour
{
    
    [SerializeField] Image characterHead;
    [SerializeField] Vector3 offset;
    [SerializeField] private LobbyNpc playerNpc;
    

     [SerializeField] List<LobbyNpc> lobbyNpcs = new List<LobbyNpc>();
    public int curNpcIndex = 0;
    public LobbyCam lobbyCam;

    public bool characterSelected = false;

    [SerializeField] private PlayableNpcController playableNpcController;
    
    // Start is called before the first frame update

    private void Awake()
    {
        PlayableCharacterData.Data.Load();
        foreach (var npc in lobbyNpcs)
        {
            npc.SetNpcManager(this);
        }
    }

    IEnumerator Start()
    {

        yield return new WaitUntil(() => DataManager.Inst);
        characterSelected = DataManager.Inst.Data.characterId >= 0;
        var _playerNpc = lobbyNpcs.FirstOrDefault(npc => npc.npcType == NpcType.Player);
        if(_playerNpc != null) playerNpc = _playerNpc;
        characterHead.gameObject.SetActive(playerNpc);
    }

    public void SetNpcPlayer(int id)
    {
        if(playerNpc != null)
            playerNpc.npcType = NpcType.Npc;
        var _playerNpc = lobbyNpcs.FirstOrDefault(npc => npc.npcId == id);
        playerNpc  = _playerNpc;
        playerNpc.npcType = NpcType.Player;
        characterHead.gameObject.SetActive(true);
    }
    
    public LobbyNpc GetPlayerNpc() => playerNpc;

    private void LateUpdate()
    {
        if(playerNpc != null)
            characterHead.transform.position = playerNpc.transform.position + offset;
    }

    public void TurningNpc(bool isRight)
    {
        curNpcIndex = isRight ? ++curNpcIndex : --curNpcIndex;
        
        if (curNpcIndex >= lobbyNpcs.Count) curNpcIndex = 0;
        else if (curNpcIndex < 0) curNpcIndex = lobbyNpcs.Count - 1;
       TurningNpc(curNpcIndex);
    }

    public void TurningNpc(int _index)
    {
        playableNpcController.panel.SetPosition(PanelStates.Hide,true);
        
        curNpcIndex = _index;
        
        PlayableCharacterData.Data selectedData =
            PlayableCharacterData.Data.DataList.FirstOrDefault(data => data.id == curNpcIndex);
        
        lobbyCam.MoveTo(lobbyNpcs[curNpcIndex].transform.position)
            .OnComplete(()=>
            {
                playableNpcController.SetNpcInfo(selectedData);
            });
       
    }

    public void SetPlayer()
    {
       
        characterSelected = true;
        SetNpcPlayer(curNpcIndex);
        lobbyCam.MoveToOrigin();
        playableNpcController.panel.SetPosition(PanelStates.Hide,true);
        string ids = PlayableCharacterData.Data.DataList.FirstOrDefault(n => n.id == lobbyNpcs[curNpcIndex].npcId)?.relicIds;
        DataManager.Inst.Data.characterId = lobbyNpcs[curNpcIndex].npcId;
        TopUIController.Inst.InstanceRelicIcon(ids,true);
        
    }

    [Button]
    public void SelectBtnPos()
    {
        playableNpcController.SetSelectBtnPos(lobbyNpcs[curNpcIndex].transform);
    }
}
