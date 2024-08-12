using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RelicManager : MonoBehaviour
{
    public List<RelicSO> relicSOs = new List<RelicSO>();
    public List<GameObject> relicLoots = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        RandomSO();
    }


    public void RandomSO()
    {
        List<int> index = new List<int>();
        for(int i = 0; i < relicLoots.Count; i++)
        {
            int random;
            do
            {
                random = Random.Range(0, relicSOs.Count);
            }
            while (index.Contains(random));
            index.Add(random);
        }

        for(int j = 0; j < relicLoots.Count; j++)
        {
            relicLoots[j].GetComponent<RelicLoot>().SetCard(relicSOs[index[j]]);
        }


    }
}
