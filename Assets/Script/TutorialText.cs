using Febucci.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    [TextArea] [SerializeField] string[] tutorialText;
    [SerializeField] TypewriterByCharacter text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Texting(int index)
    {
        text.ShowText(tutorialText[index]);
    }

    public void StopText()
    {
        text.StartDisappearingText();
    }
}
