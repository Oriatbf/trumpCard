using UnityEngine;

public static class TimeManager 
{
   public static void ChangeTimeSpeed(float timeScale)
   {
      Debug.Log(timeScale);
      Time.timeScale = timeScale;
      Time.fixedDeltaTime = 1 / Time.timeScale * 0.02f;
   }
}
