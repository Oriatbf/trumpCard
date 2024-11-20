using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] CameraSetting camSet;

    // �ʱ� ����
    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
     
    }

    // �ӽ÷� ���� �� 
    void Start()
    {
       // CountStart(); //Test
    }

    // 카운트 시작
    public void CountStart()
    {
        camSet = GameObject.Find("Camera Setting").GetComponent<CameraSetting>();
        anim.SetTrigger("countDown");
    }

    // 카운트 끝날시(자동으로 실행됨)
    public void CountEnd()
    {
        camSet.CameraAnimation();
        // 게임 시작
    }
}
