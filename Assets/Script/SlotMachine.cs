using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VHierarchy.Libs;
using VInspector;
using Random = UnityEngine.Random;
using Sequence = DG.Tweening.Sequence;


public class SlotMachine : MonoBehaviour
{
    [SerializeField] private List<RectTransform> rows = new List<RectTransform>(); // 슬롯 릴
    [SerializeField] private float speed = 200f;   // 이동 속도
    [SerializeField] private float resetPositionY; // 리셋 위치 Y (화면 위)
    [SerializeField] private float endPositionY;  // 끝 위치 Y (화면 아래)
    [SerializeField] private int price;
    
    private List<float> rowPosY = new List<float>();  
    private List<bool> rowSpins = new List<bool>();

    [SerializeField] private Panel panel;
    [SerializeField] private RandomPercent randomPercent = new RandomPercent();
    [SerializeField] private RectTransform handle,handleOut;
    [SerializeField] private Image handleOutLine;

    private bool gambling = false;
    private bool isOpen = false;

    private void Start()
    {
        for (int i = 0; i < 7; i++)
        {
            rowPosY.Add(endPositionY + (100f*i));
        }

        for (int i = 0; i < rows.Count; i++)
        {
            rowSpins.Add(false);
        }
    }
    

    public void Open()
    {
        isOpen = true;
        panel.SetPosition(PanelStates.Show,true);
    }

    public void Close()
    {
        panel.SetPosition(PanelStates.Hide,true);
        isOpen = false;
    }


    [Button]
    public void Gamble()
    {
        if (TopUIController.Inst.CurrentGold() < 50) return;
      
        float downPosY = 240;
        if(gambling) return;
        TopUIController.Inst.GetGold(price);
        Sequence handleSequence =
            DOTween.Sequence().Append(handle.DOSizeDelta(new Vector2(handle.sizeDelta.x, downPosY), 0.5f))
                .Join(handleOut.DOSizeDelta(new Vector2(handleOut.sizeDelta.x, downPosY), 0.5f))
                .Append(handle.DOSizeDelta(new Vector2(handle.sizeDelta.x, 417), 0.5f))
                .Join(handleOut.DOSizeDelta(new Vector2(handleOut.sizeDelta.x, 417), 0.5f));
        
        handleSequence.Play();
        
        gambling = true;
        for(int i= 0;i<rowSpins.Count;i++)
        {
            rowSpins[i] = true;
        }
        Percent p =  Probability.RandomProbability(randomPercent);
        
        
        p.onEvent?.Invoke();

    }

    [Button]
    public void ResetGamble()
    {
        for(int i= 0;i<rowSpins.Count;i++)
        {
            rowSpins[i] = true;
        }
    }

    public void Win()
    {
        StartCoroutine(WinCor());

    }

    private IEnumerator WinCor()
    {
        yield return new WaitForSeconds(1f);
        int random = Random.Range(0, rowPosY.Count);
        for(int i = 0;i<rows.Count;i++)
        {
            yield return new WaitForSeconds(0.5f);
            rows[i].anchoredPosition = new Vector2(rows[i].anchoredPosition.x, rowPosY[random]);
            rowSpins[i] = false;
        }

        var relics = RelicDataManager.Inst.GetRandomRelics(1);
        ShowRelicController.Inst.Show(relics);
        foreach (var relicData in relics)
        {
            TopUIController.Inst.InstanceRelicIcon(relicData,true);
        }
       

        gambling = false;
    }

    public void Fail()
    {

        StartCoroutine(FailCor());
    }

    private IEnumerator FailCor()
    {
        yield return new WaitForSeconds(1f);
        List<int> randomIndex = new List<int>();
        for(int i = 0; i<rows.Count;i++)
        {
            int random = 0;
            if (i == rows.Count - 1)
            {
                do
                {
                    random = Random.Range(0, rowPosY.Count);
                } while (randomIndex[0] == randomIndex[1] && randomIndex[1] == random);
            }
            else
                random = Random.Range(0, rowPosY.Count);
            yield return new WaitForSeconds(0.5f);
            rows[i].anchoredPosition = new Vector2(rows[i].anchoredPosition.x, rowPosY[random]);
            rowSpins[i] = false;
            randomIndex.Add(random);
        }

        gambling = false;
    }

    private void Update()
    {

        for(int i = 0;i<rows.Count;i++)
        {
            if (rowSpins[i])
            {
                // 아래로 이동
                rows[i].anchoredPosition += Vector2.down * speed * Time.deltaTime;

                // 화면 아래로 이동했을 경우, 다시 위로 이동
                if (rows[i].anchoredPosition.y <= endPositionY)
                {
                    rows[i].anchoredPosition = new Vector2(rows[i].anchoredPosition.x, resetPositionY);
                }
            }
            
        }
        
        if(isOpen)
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Close();
            }
    
   
    }

    public void OnHandle()
    {
        DOTween.Complete(handleOutLine);
        handleOutLine.DOFade(1, 0.25f);
    }

    public void ExitHandle()
    {
        DOTween.Complete(handleOutLine);
        handleOutLine.DOFade(0, 0.25f);
    }
}
