using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RelicImageText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI relicExplainText;
    // Start is called before the first frame update
    void Start()
    {
        relicExplainText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        relicExplainText.gameObject.SetActive(true);

    }

    private void OnMouseExit()
    {
        relicExplainText.gameObject.SetActive(false);

    }
}
