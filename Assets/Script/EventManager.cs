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
        

        // ��ư �ʱ�ȭ �� �ؽ�Ʈ ����
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < currentEvent.options.Length)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentEvent.options[i].ChoiceText;
                int index = i; // ���� ĸó ���� ����
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

        // ���õ� �ɼ��� ȿ�� ����
        ApplyOptionEffects(selectedOption);
    }

    private void ApplyOptionEffects(EventOption option)
    {
        // ���⿡ ���, HP, �� ���� ��ȭ�� ó���ϴ� ������ ����
        // ����:
        /*PlayerStats.Gold += option.goldChange;
        PlayerStats.HP += option.hpChange;
        PlayerStats.Strength += option.strengthChange;

        if (option.addPoison)
        {
            PlayerStats.HasPoison = true;
        }*/
    }
}
