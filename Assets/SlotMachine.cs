using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using VHierarchy.Libs;
using VInspector;
using Random = UnityEngine.Random;


public class SlotMachine : MonoBehaviour
{
    [SerializeField] private RectTransform[] rows; // 슬롯 릴
    [SerializeField] private float speed = 200f;   // 이동 속도
    [SerializeField] private float resetPositionY; // 리셋 위치 Y (화면 위)
    [SerializeField] private float endPositionY;  // 끝 위치 Y (화면 아래)
    [FormerlySerializedAs("rowPos")] [SerializeField] private List<float> rowPosY = new List<float>();
    
    public RandomPercent randomPercent = new RandomPercent();
    private bool a = false;

    private void Start()
    {
        for (int i = 0; i < 7; i++)
        {
            rowPosY.Add(endPositionY + (0.9f*i));
        }
    }


    [Button]
    public void Gamble()
    {
        Percent p =  Probability.RandomProbability(randomPercent);
        a = true;

        p.onEvent?.Invoke();

    }

    [Button]
    public void ResetGamble()
    {
        a = false;
    }

    public void Win()
    {
        int random = Random.Range(0, rowPosY.Count);
        foreach (var row in rows)
        {
            row.anchoredPosition = new Vector2(row.anchoredPosition.x, rowPosY[random]);
        }

    }

    public void Fail()
    {
        List<int> randomIndex = new List<int>();
        for(int i = 0; i<rows.Length;i++)
        {
            int random = 0;
            if (i == rows.Length - 1)
            {
                do
                {
                    random = Random.Range(0, rowPosY.Count);
                } while (randomIndex[0] == randomIndex[1] && randomIndex[1] == random);
            }
            else
                random = Random.Range(0, rowPosY.Count);
            
            rows[i].anchoredPosition = new Vector2(rows[i].anchoredPosition.x, rowPosY[random]);
            randomIndex.Add(random);
        }

    }

    private void Update()
    {
        if (!a)
        {
            foreach (var row in rows)
            {
                // 아래로 이동
                row.anchoredPosition += Vector2.down * speed * Time.deltaTime;

                // 화면 아래로 이동했을 경우, 다시 위로 이동
                if (row.anchoredPosition.y <= endPositionY)
                {
                    row.anchoredPosition = new Vector2(row.anchoredPosition.x, resetPositionY);
                }
            }
        }
       
    }
}
