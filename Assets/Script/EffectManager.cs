using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Inst;
    public GameObject[] effect;
    private void Awake()
    {
        if(Inst != this && Inst != null)
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

    public void SpawnEffect(Transform transform,int index)
    {
        GameObject a = Instantiate(effect[index],transform.position,Quaternion.identity);
        Destroy(a, 2f);
    }
}
