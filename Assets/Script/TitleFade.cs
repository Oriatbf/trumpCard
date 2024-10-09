using UnityEngine;

public class TitleFade : MonoBehaviour
{
    public Animator anim;
    private bool pressOn;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && pressOn)
        {
            pressOn = false;
            anim.SetTrigger("PressButton");
        }
    }

    public void EnablePress()
    {
        pressOn = true;
    }
}
