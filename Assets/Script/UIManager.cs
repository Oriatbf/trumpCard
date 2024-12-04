using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using DamageNumbersPro;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class UIManager : MonoBehaviour
{
    public static UIManager Inst;
    
    [SerializeField] private DamageNumber damageUI,recoverUI;

    private void Awake()
    {
        Inst = this;
    }

    public void RecorveryUI(RectTransform rect,float healAmount)
    {
        recoverUI.SpawnGUI(rect,transform.position,healAmount);
    }
    
    public void DamageUI(RectTransform rect,float damage)
    {
        damageUI.SpawnGUI(rect,transform.position,damage);
    }
}
