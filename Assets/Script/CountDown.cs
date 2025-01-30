using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private CameraSetting camSet;

    [SerializeField] private Profile leftProfile, rightProfile;
     private RectTransform leftRect;
     private RectTransform rightRect;

    // �ʱ� ����
    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        leftRect = leftProfile.GetComponent<RectTransform>();
        rightRect = rightProfile.GetComponent<RectTransform>();

    }

    // �ӽ÷� ���� �� 
    IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.Inst);
        GameManager.Inst.gamePause = true;
        rightProfile.Init(GameManager.Inst.enemyData);
        var playerData =
            PlayableCharacterData.Data.DataList.FirstOrDefault(n => n.id == DataManager.Inst.Data.characterId);
        leftProfile.Init(playerData);
       CountStart();
    }

    // 카운트 시작
    public void CountStart()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.5f)
            .Append(leftRect.DOAnchorPos(Vector2.zero, 0.25f))
            .Join(rightRect.DOAnchorPos(Vector2.zero, 0.25f))
            .AppendInterval(2.5f)
            .Append(leftRect.DOAnchorPos(new Vector2(-536.9f, 0), 0.25f))
            .Join(rightRect.DOAnchorPos(new Vector2(536.9f, 0), 0.25f));
        
        anim.SetTrigger("countDown");
        seq.Play().SetUpdate(true);
    }

    // 카운트 끝날시(자동으로 실행됨)
    public void CountEnd()
    {
        GameManager.Inst.gamePause = false;
        camSet.CameraAnimation();
        // 게임 시작
    }
}
