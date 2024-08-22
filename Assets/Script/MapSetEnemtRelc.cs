using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector.Libs;

public class MapSetEnemtRelc : MonoBehaviour
{
    public List<List<RelicSO>> enemyRelicSOs = new List<List<RelicSO>>();

    // Start is called before the first frame update
    void Start()
    {
        RelicManager relicManager = RelicManager.Inst;
        for(int i = 0; i < 10;i++)
        {
            List<RelicSO> haveSO = new List<RelicSO>(); 
            for(int j = 0; j <= i; i++)
            {
                int random;
                do
                {
                    random = Random.Range(0, relicManager.relicSOs.Count);
                }
                while (haveSO.Contains(relicManager.relicSOs[random]));
                enemyRelicSOs[i].Add(relicManager.relicSOs[random]);
            }

        }

        for (int i = 0; i < 10; i++)
        {
           
            for (int j = 0; j <= i; i++)
            {
                Debug.Log(enemyRelicSOs[i][j]);
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
