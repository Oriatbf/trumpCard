using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public  class TimeManager : MonoBehaviour
{
   public static TimeManager Inst;
   public bool timeChanging;
   private Coroutine cor;

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
         DontDestroyOnLoad(gameObject);
      }
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
