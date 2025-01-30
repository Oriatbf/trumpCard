using Map;
using Febucci.UI;
using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class EventManager : Singleton<EventManager>
{
    public Image eventImage;
    public TextMeshProUGUI eventName;
    public TextMeshProUGUI eventText;
    public Button[] optionButtons;
    public string BackText;
    public Animator eventAnim;

    private EventScriptable currentEvent;
    
    public EventScriptable[] events;



    public void StartEvent()
    {
        currentEvent = events[Random.Range(0, events.Length)];
        
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
        
        if (randomSuccess)
            Win(option);
        else 
            Lose(option);

        // Event Text Change

        if (randomSuccess)
            eventText.GetComponent<TypewriterByCharacter>().ShowText(option.AfterText);
        else
            eventText.GetComponent<TypewriterByCharacter>().ShowText(option.randomAfterText);

        optionButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = BackText;
        optionButtons[0].onClick.AddListener(() => EventClose());

    }

    private void Win(EventOption option)
    {
        switch (option.winPrize)
        {
            case EventOption.AdditionType.Gold:
                TopUIController.Inst.GetGold(option.winValue);
                break;
            case EventOption.AdditionType.Relic:
                var randomRelic = RelicDataManager.Inst.GetRandomRelics(option.winValue);
                ShowRelicController.Inst.Show(randomRelic);
                foreach (var relic in randomRelic)
                {
                    DataManager.Inst.Data.relicID.Add(relic.id);
                    TopUIController.Inst.InstanceRelicIcon(relic,true);
                }

                break;
            default:
                break;
        }
    }

    private void Lose(EventOption option)
    {
        switch (option.losePrize)
        {
            case EventOption.AdditionType.Gold:
                TopUIController.Inst.GetGold(-option.loseValue);
                break;
            case EventOption.AdditionType.Relic:
                break;
            default:
                break;
        }
    }

    private bool RandomOption(EventOption option) =>   
        Probability.RandomProbability(option.randomPercent).probabilityState == ProbabilityState.Win;


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
