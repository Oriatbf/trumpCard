using UnityEngine;

[System.Serializable]
public class EventOption
{
    [TextArea]
    public string ChoiceText;  // �������� �ؽ�Ʈ

    public enum AdditionType
    {
        Gold,
        MaxHp,
        Relic,
    }

    public AdditionType additionType;

    // Value -----------------------------------------------

    public int typeValue;     // ��ȭ��

    // Relic -----------------------------------------------

    [DrawIf("additionType", AdditionType.Relic)]
    public bool random;

    [DrawIf("additionType", AdditionType.Relic)] [DrawIf("random", false, DrawIfAttribute.DisablingType.ReadOnly)]
    public RelicSO addRelic;
}

[CreateAssetMenu(fileName = "EventSO", menuName = "Scriptable SO/Event", order = 1)]
public class EventScriptable : ScriptableObject
{   
    public Sprite eventImage;     // �̺�Ʈ �̹���
    public string eventName;      // �̺�Ʈ �̸� �ؽ�Ʈ
    public string eventText;      // �̺�Ʈ ���� �ؽ�Ʈ
    public EventOption[] options; // �̺�Ʈ ������ �迭
}
