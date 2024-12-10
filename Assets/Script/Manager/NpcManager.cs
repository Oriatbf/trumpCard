using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NpcManager : MonoBehaviour
{
    public static NpcManager Inst;
    
    [SerializeField] Image characterHead;
    [SerializeField] Vector3 offset;
    [SerializeField] private LobbyNpc playerNpc;

     [SerializeField] List<LobbyNpc> lobbyNpcs = new List<LobbyNpc>();

    public bool playerMoving;

    private void Awake()
    {
        Inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        lobbyNpcs.AddRange(FindObjectsOfType<LobbyNpc>());
        var _playerNpc = lobbyNpcs.Where((npc => npc.npcType == NpcType.Player)).ToList();
        if(_playerNpc.Count>=2) Debug.LogError("활성화된 플레이어가 두명임");
        playerNpc = _playerNpc[0];

    }

    public void SetNpcPlayer()
    {
        playerNpc.npcType = NpcType.Npc;
        var _playerNpc = lobbyNpcs.Where((npc => npc.npcType == NpcType.Player)).ToList();
        if(_playerNpc.Count>=2) Debug.LogError("활성화된 플레이어가 두명임");
        playerNpc = _playerNpc[0];
    }
    
    
   

    public void StopMoving()
    {
        playerMoving = false;
    }
    

    public LobbyNpc GetPlayerNpc() => playerNpc;

    private void LateUpdate()
    {
        characterHead.transform.position = playerNpc.transform.position + offset;
    }
}
