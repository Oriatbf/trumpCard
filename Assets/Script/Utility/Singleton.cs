using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T inst;

    public static T Inst
    {
        get
        {
            if (inst) return inst;
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            inst = (T)FindFirstObjectByType(typeof(T));
 
            if (inst) return inst;
            var obj = new GameObject(typeof(T).Name, typeof(T));
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            inst = obj.GetComponent<T>();
             
            return inst;
        }
    }
    

}
