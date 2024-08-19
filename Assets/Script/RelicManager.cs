using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RelicManager : MonoBehaviour
{
    public static RelicManager Inst;

    public List<RelicSO> relicSOs = new List<RelicSO>();

    public List<RelicSO> commonRelicSOs = new List<RelicSO>();
    public List<RelicSO> unCommonRelicSOs = new List<RelicSO>();
    public List<RelicSO> rareRelicSOs = new List<RelicSO>();
  //  public List<RelicSO> epicRelicSOs = new List<RelicSO>();
  //  public List<RelicSO> legendaryRelicSOs = new List<RelicSO>();
    public List<int> rarityChance;
    [SerializeField] List<int> rarityChanceList;

    [SerializeField] private Transform relicLootsLayout;
    public List<GameObject> relicLoots = new List<GameObject>();

    private void Awake()
    {
        Inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        int sum = 0;
        foreach(var num in rarityChance)
        {
            sum+= num;
            rarityChanceList.Add(sum);
        }
        if (sum != 1000) Debug.Log("유물 확률 문제");

        


        for(int i = 0;i< relicLootsLayout.childCount; i++)
        {
            relicLoots.Add(relicLootsLayout.GetChild(i).gameObject);
        }
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
                random = Random.Range(1, 1001);
            }
            while (index.Contains(random));
            index.Add(random);
        }
        List<int> index2 = new List<int>();

        for (int j = 0; j < relicLoots.Count; j++)
        {
            int k = 0;
            List<RelicSO> curRarityRelics = new List<RelicSO>();
            for (k = 0; k<rarityChance.Count; k++)
            {
                if (index[j] <= rarityChanceList[k])
                {
                    break;
                }
            }
            switch (k)
            {
                case 0:
                    curRarityRelics = commonRelicSOs; break;
                case 1:
                    curRarityRelics = unCommonRelicSOs; break;
                case 2:
                    curRarityRelics = rareRelicSOs; break;
            }
            int random;
            do
            {
                random = Random.Range(0,curRarityRelics.Count );
            }
            while (index2.Contains(random));
            index2.Add(random);

            relicLoots[j].GetComponent<RelicLoot>().SetCard(curRarityRelics[random]);
        }


    }
}
