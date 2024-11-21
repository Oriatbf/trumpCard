using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using VInspector;

public class RelicManager : MonoBehaviour
{
    public static RelicManager Inst;

    public List<RelicSO> AllRelics = new List<RelicSO>();
    public List<RelicSO> relicSOs = new List<RelicSO>();
    
    public CardStats playerSO;
    public List<RelicSO> playerRelic = new List<RelicSO>();
    public List<List<RelicSO>> enemyRelicSOs = new List<List<RelicSO>>(); //디버깅
    public List<RelicSO> enemyRelic = new List<RelicSO>(); //디버깅
    [SerializeField] private  List<RelicSO> commonRelicSOs = new List<RelicSO>();
    [SerializeField]private  List<RelicSO> rareRelicSOs = new List<RelicSO>();
    [SerializeField]private  List<RelicSO> epicRelicSOs = new List<RelicSO>();

    [SerializeField] List<int> rarityChance;

    [SerializeField] private Transform relicLootsLayout;
    public List<GameObject> relicLoots = new List<GameObject>();

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
        
        var relics = Resources.LoadAll<RelicSO>("Relics");
        AllRelics = new List<RelicSO>(relics);
        commonRelicSOs = AllRelics.Where(relic => relic.rarity == RelicSO.Rarity.Common).ToList();
        rareRelicSOs = AllRelics.Where(relic => relic.rarity == RelicSO.Rarity.Rare).ToList();
        epicRelicSOs = AllRelics.Where(relic => relic.rarity == RelicSO.Rarity.Epic).ToList();

       
        for (int i = 0; i < relicLootsLayout.childCount; i++)
        {
            relicLoots.Add(relicLootsLayout.GetChild(i).gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        /*
        playerSO.ClearDebuffList();

        foreach (var relic in playerRelic) //처음으로 맵선택 왔을 때 인챈트 렐릭
        {
            InChantRelic inChantRelic = relic as InChantRelic;
            if (inChantRelic != null)
            {
                foreach (InChantRelic.RelicType relicType in inChantRelic.relicType)
                    inChantRelic.Inchant(playerSO, relicType);
            }

        }*/

        for (int i = 0; i < 10; i++)
        {
            enemyRelicSOs.Add(GetRandomRelics(i + 1));
        }

        enemyRelic = enemyRelicSOs[0];


    }

    [Button]
    public void RelicDebug() 
    {
        Debug.Log(playerRelic.Count);
        Debug.Log(playerRelic[1]);
    }

    [Button]
    public void SetRelic()
    {
        var randomRelics = GetRandomRelics(relicLoots.Count);
        for (int i = 0; i < relicLoots.Count; i++)
        {
            relicLoots[i].GetComponent<RelicLoot>().SetCard(randomRelics[i]);
        }
    }


    public List<RelicSO> GetRandomRelics(int _count)
    {
        HashSet<RelicSO> returnRelicGroup = new HashSet<RelicSO>();
        for (int i = 0; i < _count; i++)  //count 만큼 반복
        {
            RelicSO randomRelic = null;
            do
            {
                int rarity = Random.Range(1, 1001);
                int j = 0;
                for (j = 0; j < rarityChance.Count; j++)
                {
                    if (rarity <= rarityChance[j]) break;
                }

                switch (j)
                {
                    case 0:
                        randomRelic = RandomRelicInGroup(commonRelicSOs);
                        break;
                    case 1:
                        randomRelic = RandomRelicInGroup(rareRelicSOs);
                        break;
                    case 2:
                        randomRelic = RandomRelicInGroup(epicRelicSOs);
                        break;
            
                }

             }while(!returnRelicGroup.Add(randomRelic));
                
        
            returnRelicGroup.Add(randomRelic);
        }

        return returnRelicGroup.ToList();
    }

    private RelicSO RandomRelicInGroup(List<RelicSO> relicGroup)
    {
        int randomIndex = Random.Range(0,relicGroup.Count);
        return relicGroup[randomIndex];
    }
   
    public (List<RelicSO> curRarityRelics, int random) RandomSO(int relicCount)
    {
        int sum = 0;

        Debug.Log("RandomSo");
        List<int> index = new List<int>();
        for(int i = 0; i < relicCount; i++)
        {
            Debug.Log("RandomSo");
            int random;
            do
            {
                random = Random.Range(1, 1001);
            }
            while (index.Contains(random));
            index.Add(random);
        }
        List<int> index2 = new List<int>();

        for (int j = 0; j < relicCount; j++)
        {
            int k = 0;
            List<RelicSO> curRarityRelics = new List<RelicSO>();
            for (k = 0; k<rarityChance.Count; k++)
            {
                Debug.Log(index.Count);

            }
            switch (k)
            {
                case 0:
                    curRarityRelics = commonRelicSOs; break;
                case 1:
                    curRarityRelics = rareRelicSOs; break;
                case 2:
                    curRarityRelics = epicRelicSOs; break;
            }
            int random;
            do
            {
                random = Random.Range(0,curRarityRelics.Count );
            }
            while (index2.Contains(random));
            index2.Add(random);
            List<RelicSO> cur_relicSOs = new List<RelicSO>();
            cur_relicSOs.Add(curRarityRelics[random]);
            Debug.Log(curRarityRelics[random]);

            return (curRarityRelics, random);
        }

        return (null, 0);
    }
}
