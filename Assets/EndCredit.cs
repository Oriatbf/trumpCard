using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EndCredit : MonoBehaviour
{
    public Button skipBtn;
    bool pressOn;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !pressOn)
        {
            pressOn = true;
            skipBtn.GetComponent<Image>().DOFade(1f, 1f);

            skipBtn.onClick.AddListener(() => Skip());
        }
    }

    void Skip()
    {
        GameManager.Inst.SceneTransition("LobbyScene");
    }
}
