using DG.Tweening;
using UnityEngine;

public class TitleFade : MonoBehaviour
{
    public Animator anim;
    [SerializeField] private PlayableNpcController playableNpcController;
    private bool pressOn;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && pressOn)
        {
            pressOn = false;
            anim.SetTrigger("PressButton");
            if(DataManager.Inst.Data.characterId<0)
                DOVirtual.DelayedCall(0.8f, () => playableNpcController.characterSelectTexting());
        }
    }

    public void EnablePress()
    {
        pressOn = true;
    }
}
