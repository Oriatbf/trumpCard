using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector.Libs;

public class MapSetEnemtRelc : MonoBehaviour
{
    public static MapSetEnemtRelc Inst;
    public List<List<RelicSO>> enemyRelicSOs = new List<List<RelicSO>>();
    private void Awake()
    {
        if (Inst != this && Inst != null)
        {
            return;
        }
        else
        {
            Inst = this;
        }
    }

   

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("llll");
        RelicManager relicManager = RelicManager.Inst;
        for (int i = 0; i < 10; i++)
        {
            enemyRelicSOs.Add(new List<RelicSO>());
            List<RelicSO> haveSO = new List<RelicSO>();
            for (int j = 0; j <= i; j++)
            {
                int random;
                do
                {
                    random = Random.Range(0, relicManager.relicSOs.Count);
                }
                while (haveSO.Contains(relicManager.relicSOs[random]));
                Debug.Log(relicManager.relicSOs[random]);
                haveSO.Add(relicManager.relicSOs[random]); // Add the selected relic to the list to avoid duplicates
                enemyRelicSOs[i].Add(relicManager.relicSOs[random]);
               
            }
        }

        int p = 0;
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                p++;
            }
        }
        Debug.Log(p);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
