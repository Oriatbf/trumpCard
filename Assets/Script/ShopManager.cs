using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Inst;

    [SerializeField] private Transform relicLootsLayout;
    [SerializeField] private Animator shopAnim;
    [SerializeField] private Animator mapAnim;
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

        for (int i = 0; i < relicLootsLayout.childCount; i++)
        {
            relicLoots.Add(relicLootsLayout.GetChild(i).gameObject);
        }

        shopAnim = gameObject.GetComponent<Animator>();
        //mapAnim = GameObject.Find("Scroll View Horizontal").GetComponent<Animator>();
    }

    void Refresh()
    {
        for (int i = 0; i < relicLoots.Count; i++)
        {
            var SO = RelicManager.Inst.RandomSO(relicLoots);
            relicLoots[i].GetComponent<RelicLoot>().SetCard(SO.curRarityRelics[SO.random]);
        }
    }

    public void RefreshBtn()
    {
        if (UIManager.Inst.gold >= 70)
        {
            UIManager.Inst.gold -= 70;
            Refresh();
        }
    }

    [ContextMenu("TestShopOpen")]
    public void ShopOpen()
    {
        shopAnim.SetBool("ShopFade", true);
        mapAnim.SetBool("MapFade", true);
        Refresh();
    }

    public void ShopClose()
    {
        shopAnim.SetBool("ShopFade", false);
        mapAnim.SetBool("MapFade", false);
        MapPlayerTracker.Instance.Locked = false;
    }
}
