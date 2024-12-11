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
    public List<Card> relicLoots = new List<Card>();

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
            relicLoots.Add(relicLootsLayout.GetChild(i).GetComponent<Card>());
        }

        shopAnim = gameObject.GetComponent<Animator>();
        //mapAnim = GameObject.Find("Scroll View Horizontal").GetComponent<Animator>();
    }

    void Refresh()
    {
        
        var randomRelics = RelicDataManager.Inst.GetRandomRelics(relicLoots.Count);
        
        for (int i = 0; i < relicLoots.Count; i++)
        {
            relicLoots[i].Init(randomRelics[i]);
            relicLoots[i].purchased = false;
        }
    }

    public void RefreshBtn()
    {
        if (TopUIController.Inst.CurrentGold() >= 70)
        {
            TopUIController.Inst.GetGold(-70);
            Refresh();
        }
    }

    [ContextMenu("ShopOpen")]
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
