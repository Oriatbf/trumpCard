using System;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class LobbyCam : MonoBehaviour
{
    private CinemachineVirtualCamera _cam;
    private Vector3 originVector;

    private void Awake()
    {
        _cam = GetComponent<CinemachineVirtualCamera>();
        originVector = transform.position;
    }

    public Tween MoveTo(Vector2 vector2,float duration = 0.6f)
    {
        CameraZoom(2.2f);
        return transform.DOMove(new Vector3(vector2.x + 1f, vector2.y, -10), duration);
    }

    public void MoveToOrigin()
    {
        transform.DOMove(originVector, .6f);
        CameraZoom(5);
    }

    private void CameraZoom(float endValue,float duration = 0.6f)
    {
        DOTween.To(() => _cam.m_Lens.OrthographicSize, x => _cam.m_Lens.OrthographicSize = x, endValue, duration);
    }
}
