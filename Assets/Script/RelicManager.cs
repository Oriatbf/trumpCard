using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using VInspector;

public class RelicManager : MonoBehaviour
{
    public static RelicManager Inst;
    public List<RelicSO> playerRelic = new List<RelicSO>();
    public List<int> playerBloodInchant = new List<int>();
    public List<RelicSO> enemyRelic = new List<RelicSO>();
    public List<RelicSO> relicSOs = new List<RelicSO>();
    public List<RelicSO> cur_relicSOs = new List<RelicSO>();
    public List<RelicSO> commonRelicSOs = new List<RelicSO>();
    public List<RelicSO> unCommonRelicSOs = new List<RelicSO>();
    public List<RelicSO> rareRelicSOs = new List<RelicSO>();
    public List<RelicSO> InchantRelics= new List<RelicSO>();

    public List<int> rarityChance;
    [SerializeField] List<int> rarityChanceList;

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

        foreach (var item in InchantRelics)
        {
 
            InChantRelic inChantRelic = item as InChantRelic;
            inChantRelic.excute.isExcute = false;
            
        }
       
        for (int i = 0; i < relicLootsLayout.childCount; i++)
        {
            relicLoots.Add(relicLootsLayout.GetChild(i).gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    [Button]
    public void RelicDebug() 
    {
        Debug.Log(playerRelic.Count);
        Debug.Log(playerRelic[1]);
    }

    [Button]
    public void GameStart()
    {

    }

    public void InchantBlood(CardStats so)
    {
        if(playerBloodInchant.Count> 0)
        {
            for (int i = 0; i < playerBloodInchant.Count; i++)
            {
                if (playerBloodInchant[i] == so.infor.cardNum)
                {
                    so.relicInfor.bloodSucking = true;
                    break;
                }
            }
        }
      
    }

   
    public (List<RelicSO> curRarityRelics, int random) RandomSO(int relicCount)
    {
        int sum = 0;
        foreach (var num in rarityChance)
        {
            sum += num;
            rarityChanceList.Add(sum);
        }
        if (sum != 1000) Debug.Log("유물 확률 문제");

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
                Debug.Log(rarityChanceList[k]);
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
            cur_relicSOs.Add(curRarityRelics[random]);
            Debug.Log(curRarityRelics[random]);

            return (curRarityRelics, random);
        }

        return (null, 0);
    }
}
