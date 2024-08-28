using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] Image characterHead;
    [SerializeField] Vector3 offset;
    GameObject player;

    public bool playerMoving;
    // Start is called before the first frame update
    void Start()
    {
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

    private void LateUpdate()
    {
        characterHead.transform.position = player.transform.position + offset;
    }
}
