using UnityEngine;

[System.Serializable]
public class EventOption
{
    [TextArea]
    public string ChoiceText;  // 선택지의 텍스트

    public enum AdditionType
    {
        Gold,
        MaxHp,
        Relic,
    }

    public AdditionType additionType;

    // Value -----------------------------------------------

    public int typeValue;     // 변화량

    // Relic -----------------------------------------------

    [DrawIf("additionType", AdditionType.Relic)]
    public bool random;

    [DrawIf("additionType", AdditionType.Relic)] [DrawIf("random", false, DrawIfAttribute.DisablingType.ReadOnly)]
    public RelicSO addRelic;
}

[CreateAssetMenu(fileName = "EventSO", menuName = "Scriptable SO/Event", order = 1)]
public class EventScriptable : ScriptableObject
{   
    public Sprite eventImage;     // 이벤트 이미지
    public string eventName;      // 이벤트 이름 텍스트
    public string eventText;      // 이벤트 설명 텍스트
    public EventOption[] options; // 이벤트 선택지 배열
}
