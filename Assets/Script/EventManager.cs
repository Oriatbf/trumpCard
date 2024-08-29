using Map;
using Febucci.UI;
using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    public static EventManager Inst;

    public Image eventImage;
    public TextMeshProUGUI eventName;
    public TextMeshProUGUI eventText;
    public Button[] optionButtons;
    public string BackText;
    public Animator eventAnim;

    private EventScriptable currentEvent;

    public EventScriptable[] playerRelic;

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
    }

    public void StartEvent()
    {
        currentEvent = playerRelic[Random.Range(0, playerRelic.Length)];
        eventImage.sprite = currentEvent.eventImage;

        eventName.GetComponent<TypewriterByCharacter>().ShowText(currentEvent.eventName);
        eventText.GetComponent<TypewriterByCharacter>().ShowText(currentEvent.eventText);
        eventImage.DOFade(1f, 1f);

        // Button Change
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < currentEvent.options.Length)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionButtons[i].GetComponentInChildren<Image>().DOFade(1f, 1f);
                optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentEvent.options[i].ChoiceText;
                int index = i;
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
        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].onClick.RemoveAllListeners();
        }
        for (int i = 1; i < optionButtons.Length; i++)
        {
            optionButtons[i].gameObject.SetActive(false);
        }

        EventOption selectedOption = currentEvent.options[index];

        ApplyOptionEffects(selectedOption);
    }

    private void ApplyOptionEffects(EventOption option)
    {
        bool randomSuccess = RandomOption(option);

        switch (option.additionType)
        {
            case EventOption.AdditionType.Gold:
                if (randomSuccess)
                    UIManager.Inst.GoldCount(option.typeValue);
                break;
            case EventOption.AdditionType.Relic:
                if (randomSuccess)
                {
                    if(option.randomRelic)
                    {
                        for (int i = 0; i < option.typeValue; i++)
                        {
                            var SO = RelicManager.Inst.RandomSO(option.typeValue);
                            RelicManager.Inst.playerRelic.Add(SO.curRarityRelics[SO.random]);
                            UIManager.Inst.InstanceRelicIcon(SO.curRarityRelics[SO.random]);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < option.typeValue; i++)
                        {
                            RelicSO RanSO = option.Relics[Random.Range(0, option.Relics.Length)];
                            RelicManager.Inst.playerRelic.Add(RanSO);
                            UIManager.Inst.InstanceRelicIcon(RanSO);
                        }
                    }
                }
                break;
            default:
                break;
        }

        // Event Text Change

        if (randomSuccess)
            eventText.GetComponent<TypewriterByCharacter>().ShowText(option.AfterText);
        else
            eventText.GetComponent<TypewriterByCharacter>().ShowText(option.randomAfterText);

        optionButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = BackText;
        optionButtons[0].onClick.AddListener(() => EventClose());

    }

    private bool RandomOption(EventOption option)
    {
        if (!option.random || Random.Range(1, 101) >= option.randomValue)
        {
            return true;
        }

        switch (option.randomAddType)
        {
            case EventOption.AdditionType.Gold:
                if(option.allGold_Remove)
                    UIManager.Inst.GoldCount(-UIManager.Inst.gold); Debug.Log("다슴");
                break;
            case EventOption.AdditionType.Relic:
                break;
            default:
                break;
        }

        return false;
    }

    [ContextMenu("asf")]
    public void EventOpen()
    {
        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].gameObject.SetActive(false);
            optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
            optionButtons[i].GetComponentInChildren<Image>().DOFade(0f, 0f);
        }

        eventText.text = "";
        eventName.text = "";

        eventImage.DOFade(0f, 0f);

        eventAnim.SetBool("EventFade", true);
        Invoke("StartEvent", 1f);
    }

    public void EventClose()
    {
        optionButtons[0].onClick.RemoveAllListeners();
        eventAnim.SetBool("EventFade", false);
        MapPlayerTracker.Instance.Locked = false;
    }
}
