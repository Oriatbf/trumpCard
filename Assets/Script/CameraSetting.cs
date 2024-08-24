using UnityEngine;
using Cinemachine;
using DG.Tweening;
using VInspector;

public class CameraSetting : MonoBehaviour
{
    [Tab("Settings")]
    [ReadOnly] [SerializeField] CinemachineVirtualCamera virtualCamera;
    [ReadOnly] [SerializeField] Transform player;

    /*[Tab("Modify")]
    [Range(1, 10)] [SerializeField] float startSize = 5.5f;
    [Range(1, 10)] [SerializeField] float endSize = 5;
    [Range(0, 5)] [SerializeField] float duration = 1;*/

    // �ʱ� ����
    private void Awake()
    {
        virtualCamera = GameObject.Find("VCam").GetComponent<CinemachineVirtualCamera>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();

        //virtualCamera.m_Lens.OrthographicSize = startSize;
    }

    // �ִϸ��̼� ����
    public void CameraAnimation() 
    {
        virtualCamera.Follow = player;

        /*DOTween.To(() => virtualCamera.m_Lens.OrthographicSize,
                   x => virtualCamera.m_Lens.OrthographicSize = x, endSize, duration);*/
    }
}
