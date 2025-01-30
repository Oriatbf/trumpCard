using Unity.VisualScripting;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < CardDataManager.Inst.cardDatas.Count; i++)
        {
            CardDataManager.Inst.cardDatas[i].stat.buffDebuffValue.hp += 100;
        }
    }
    
    

}
