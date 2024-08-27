using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public Image eventImage;
    public TextMeshProUGUI eventName;
    public TextMeshProUGUI eventText;
    public Button[] optionButtons;

    private EventScriptable currentEvent;

    public EventScriptable test;

    TypewriterByCharacter typer;

    [ContextMenu("Test")]
    public void Test()
    {
        StartEvent(test);
    }
    public void StartEvent(EventScriptable newEvent)
    {
        currentEvent = newEvent;
        eventImage.sprite = currentEvent.eventImage;
        eventText.text = "";
        eventName.text = "";

        eventText.GetComponent<TypewriterByCharacter>().ShowText(currentEvent.eventText);
        

        // 버튼 초기화 및 텍스트 설정
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < currentEvent.options.Length)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentEvent.options[i].ChoiceText;
                int index = i; // 람다 캡처 문제 방지
                optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnOptionSelected(int index)
    {
        EventOption selectedOption = currentEvent.options[index];

        // 선택된 옵션의 효과 적용
        ApplyOptionEffects(selectedOption);
    }

    private void ApplyOptionEffects(EventOption option)
    {
        // 여기에 골드, HP, 힘 등의 변화를 처리하는 로직을 구현
        // 예시:
        /*PlayerStats.Gold += option.goldChange;
        PlayerStats.HP += option.hpChange;
        PlayerStats.Strength += option.strengthChange;

        if (option.addPoison)
        {
            PlayerStats.HasPoison = true;
        }*/
    }
}
