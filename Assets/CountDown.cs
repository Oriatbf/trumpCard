using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] CameraSetting camSet;

    // 초기 세팅
    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        camSet = GameObject.Find("Camera Setting").GetComponent<CameraSetting>();
    }

    // 임시로 넣은 거 
    void Start()
    {
        CountStart(); //Test
    }

    // 카운트 시작
    public void CountStart()
    {
        anim.SetTrigger("countDown");
    }

    // 카운트 끝날 시
    public void CountEnd()
    {
        camSet.CameraAnimation();
        // 플레이어 움직일 수 있게
    }
}
