using System;
using System.Collections.Generic;
using Febucci.UI.Core;
using TMPro;
using UnityEngine;

public class MapTutorial : MonoBehaviour
{
    [SerializeField] private TypewriterCore explainText;
    [SerializeField] private Transform content;
    private List<TutorialCard> cards = new List<TutorialCard>();
    [SerializeField] private Panel panel;

    private bool isOpen;

    private void Start()
    {
        foreach (Transform child in content)
        {
            var t_card = child.GetComponent<TutorialCard>();
            t_card.SetMapTutorial(this);
            cards.Add(t_card);
            
        }
    }

    private void Update()
    {
        if (isOpen)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Close();
            }
        }
    }

    public void Open()
    {
        string s = "카드를 선택";
        Texting(s);
        isOpen = true;
        panel.SetPosition(PanelStates.Show,true);
    }
    
    private void Close()
    {
        isOpen = false;
        panel.SetPosition(PanelStates.Hide,true);

    }

    public void Texting(string _text)
    {
        explainText.ShowText(_text);
    }
}
