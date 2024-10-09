using UnityEngine;

public class MoveRectTransform : MonoBehaviour
{
    [SerializeField] RectTransform targetRectTransform;

    private void Awake()
    {
        targetRectTransform = gameObject.GetComponent<RectTransform>();
    }
    public void SetPosY(float newY)
    {
        if (targetRectTransform != null)
        {
            Vector2 newPosition = targetRectTransform.anchoredPosition;
            newPosition.y = newY;
            targetRectTransform.anchoredPosition = newPosition;
        }
    }
}