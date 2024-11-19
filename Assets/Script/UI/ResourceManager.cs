using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private GameObject relicIcon;
    [SerializeField] private Transform relicIconCanvas;
    [SerializeField] private Canvas uiTopBar;

    private int _gold = 0;
    
    public void InstanceRelicIcon(RelicSO relicSO)
    {
        GameObject icon =  Instantiate(relicIcon, relicIconCanvas);
        icon.GetComponent<Image>().sprite = relicSO.relicIcon;
        icon.transform.Find("RelicLootImage").GetComponent<RelicLoot>().SetCard(relicSO);
    }

    public void GetGold(int value)
    {
        _gold += value;
    }

    public void ResetResources()
    {
        foreach (Transform _child in relicIconCanvas)
        {
           Destroy(_child.gameObject);
        }

        _gold = 0;
    }
}
