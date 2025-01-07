using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Image = UnityEngine.UI.Image;
using Sequence = DG.Tweening.Sequence;

public class PlayableNpcController : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
    public  Panel infoPanel,txtPanel;
    [SerializeField] private Transform relicContent;
    [SerializeField] private Image select;

    [SerializeField] private TextMeshProUGUI selectCharacterTxt;
    private RelicIcon relicIcon;
    private PlayableNpcManager playableNpcManager;

    private void Awake()
    {
        relicIcon = Resources.Load<RelicIcon>("UIPrefab/Icon");
    }



    public void characterSelectTexting()
    {
        txtPanel.SetPosition(PanelStates.Show,true);
        TextAlphaPingpong(selectCharacterTxt,0.5f);
    }

    public void SetSelectBtnPos(Transform _transform)
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(_transform.position);
        select.transform.position = pos;
    }


    public void SetNpcInfo(PlayableCharacterData.Data data)
    {
        infoPanel.SetPosition(PanelStates.Show,true);
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

    public void TextAlphaPingpong(TextMeshProUGUI _text,float _duration)
    {
        Sequence alphaPingpong = DOTween.Sequence()
            .Append(DOTween.To(() => _text.alpha, x => _text.alpha = x, 0f, _duration).SetEase(Ease.InCubic))
            .Append(DOTween.To(() => _text.alpha, x => _text.alpha = x, 1f, _duration).SetEase(Ease.InCubic))
            .SetLoops(-1);
        alphaPingpong.Play();
    }
}
