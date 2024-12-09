using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager Inst;
    [SerializeField] private Volume volume;
    private ColorAdjustments colorAdjustments;
    private ChromaticAberration chroAberr;
    
    private void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Inst = this;
        }
    }

    private void Start()
    {
        if (volume != null && volume.profile.TryGet(out colorAdjustments)) { }
        if (volume != null && volume.profile.TryGet(out chroAberr)) { }

    }

    public void SandevistanEffect(float time)
    {
        StartCoroutine(SVExcute(time));
    }

    IEnumerator SVExcute(float time)
    {
        float curTime = 0;
        DOTween.To(() => chroAberr.intensity.value, 
            x => chroAberr.intensity.value = x, 0.3f, 0.5f).SetUpdate(true);
        while (curTime < time)
        {
            curTime += Time.unscaledDeltaTime;
            colorAdjustments.hueShift.value = Mathf.PingPong(curTime * 360 / time, 360) - 180;
            yield return null;
        }
        DOTween.To(() => chroAberr.intensity.value, 
            x => chroAberr.intensity.value = x, 0f, 0.5f).SetUpdate(true);
        colorAdjustments.hueShift.value = 0;
    }
    
}
