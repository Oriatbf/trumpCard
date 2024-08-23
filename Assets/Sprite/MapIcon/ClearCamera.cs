using UnityEngine;

public class ClearCamera : MonoBehaviour
{
    void OnPreRender()
    {
        GL.Clear(true, true, Color.black);
    }
}