using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class PlayableNpcController : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
    public Panel panel;
    [SerializeField] private Transform relicContent;
    [SerializeField] private Image select;
    private RelicIcon relicIcon;
    private PlayableNpcManager playableNpcManager;

    private void Awake()
    {
        relicIcon = Resources.Load<RelicIcon>("UIPrefab/Icon");
    }

    private void Update()
    {
        
    }

    public void SetSelectBtnPos(Transform _transform)
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(_transform.position);
        select.transform.position = pos;
    }


    public void SetNpcInfo(PlayableCharacterData.Data data)
    {
        panel.SetPosition(PanelStates.Show,true);
        texts[0].text = $"이름 : {data.name}";
        texts[1].text = $"성별 : {data.gender}";
        texts[2].text = $"나이 : {data.age}";
        texts[3].text = $"정보 : {data.info}";
        texts[4].text = $"보유 유물";

        if (relicContent.childCount > 0)
        {
            foreach (Transform child in relicContent)
            {
                Destroy(child.gameObject);
            }
        }
        
        List<RelicDatas> relicDatas = new List<RelicDatas>();
        relicDatas = RelicDataManager.Inst.GetRelics(data.relicIds);
        foreach (var relic in relicDatas)
        {
            RelicIcon _icon =  Instantiate(relicIcon, relicContent);
            _icon.Init(relic);
        }

    }
}
