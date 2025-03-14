using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public  class TimeManager : SingletonDontDestroyOnLoad<TimeManager>
{
   public bool timeChanging = false;
   private Coroutine cor;

   protected override void Awake()
   {
      base.Awake();
   }

   public void ChangeTimeSpeed(float timeScale)
   {
      Debug.Log(timeScale);
      Time.timeScale = timeScale;
      Time.fixedDeltaTime = 1 / Time.timeScale * 0.02f;
   }
   
   public void ChangeTimeSpeedCor(float timeScale,float time)
   {
      if (cor != null)
      {
         StopCoroutine(cor);
      }
      timeChanging = true;
      Debug.Log(timeScale);
      Time.timeScale = timeScale;
      Time.fixedDeltaTime = 1 / Time.timeScale * 0.02f;
      cor = StartCoroutine(ChangeTime(time));
   }

   private IEnumerator ChangeTime(float time)
   {
      yield return new WaitForSecondsRealtime(time);
      timeChanging = false;
      ChangeTimeSpeed(1f);
   }
   

}
