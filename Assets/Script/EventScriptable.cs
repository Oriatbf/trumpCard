using System;
using UnityEngine;
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

    public AdditionType additionType;

    // Value -----------------------------------------------

    public int typeValue;

    [DrawIf("randomAddType", AdditionType.Gold)]
    public bool ImsiRelic;

    // Random -----------------------------------------------

    public bool random;

    [DrawIf("random", true)]
    public int randomValue;

    [DrawIf("random", true)]
    public AdditionType randomAddType; // 1 ~ 100

    [DrawIf("random", true)] [DrawIf("randomAddType", AdditionType.Gold)]
    public bool allGold_Remove;

    [DrawIf("random", true)] public string randomAfterText;

    // Relic -----------------------------------------------

    [DrawIf("additionType", AdditionType.Relic)]
    public bool randomRelic;

    public RelicSO[] Relics;
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
