using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    [SerializeField] private float time =2;
     void Start()
     {
         Destroy(gameObject,time);
     }
 
 }
