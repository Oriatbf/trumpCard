using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameTxt, characteristicTxt, determinTxt;

    [SerializeField] private Image profile;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Init(PlayableCharacterData.Data _data)
    {
        nameTxt.text = _data.name;
        characteristicTxt.text = _data.info;
        determinTxt.text = _data.determin;

    }
    
    public void Init(NpcDataManager.Data _data)
    {
        nameTxt.text = _data.name;
        characteristicTxt.text = _data.description;
        determinTxt.text = _data.determin;
    }
}
