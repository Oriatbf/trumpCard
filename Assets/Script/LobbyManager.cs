using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] Image characterHead;
    [SerializeField] Vector3 offset;
    GameObject player;

     [SerializeField] LobbyNpc[] lobbyNpcs = new LobbyNpc[0];

    public bool playerMoving;
    // Start is called before the first frame update
    void Start()
    {
        lobbyNpcs = FindObjectsOfType<LobbyNpc>();
        player = GameObject.FindGameObjectWithTag("LobbyPlayer");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopMoving()
    {
        playerMoving = false;
    }

    public void CheckNpc()
    {
        foreach(var npc in lobbyNpcs)
        {
            npc.NpcInteract();
        }
    }

    private void LateUpdate()
    {
        characterHead.transform.position = player.transform.position + offset;
    }
}
