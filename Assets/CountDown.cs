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
        camSet = GameObject.Find("Camera Setting").GetComponent<CameraSetting>();
    }

    // �ӽ÷� ���� �� 
    void Start()
    {
        CountStart(); //Test
    }

    // ī��Ʈ ����
    public void CountStart()
    {
        anim.SetTrigger("countDown");
    }

    // ī��Ʈ ���� ��
    public void CountEnd()
    {
        camSet.CameraAnimation();
        // �÷��̾� ������ �� �ְ�
    }
}
