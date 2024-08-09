using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Inst;

    public bool  isInGame; //게임 중 인지 확인

    private void Awake()
    {
        Inst = this;
        isInGame= true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EndGamble()
    {
 
    }
}
