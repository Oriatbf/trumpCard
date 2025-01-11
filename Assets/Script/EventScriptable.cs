using System;
using UnityEngine;
using UnityEngine.Serialization;
using VInspector;

[Serializable]
public class EventOption
{
    [TextArea]
    public string ChoiceText;

    [TextArea]
    public string AfterText;

    public enum AdditionType
    {
        None,
        Gold,
        Relic,
    }
    public AdditionType winPrize;
    public AdditionType losePrize;

    // Value -----------------------------------------------
    public int winValue; //승리했을때 얻는 값
    public int loseValue; //졌을 때 잃는 값
    
    // Random -----------------------------------------------
    
    
    public RandomPercent randomPercent = new RandomPercent();

    

    [DrawIf("random", true)] 
    public string randomAfterText;


   
    
}

[CreateAssetMenu(fileName = "EventSO", menuName = "Scriptable SO/Event", order = 1)]
public class EventScriptable : ScriptableObject
{   
    public Sprite eventImage;
    public string eventName;

    [TextArea]
    public string eventText;

    public EventOption[] options;
}
