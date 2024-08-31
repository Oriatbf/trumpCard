using Febucci.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    public static TutorialText Inst;

    [TextArea] [SerializeField] string[] tutorialText;
    [SerializeField] TypewriterByCharacter text;
    public bool disableAction;

    private void Awake()
    {
        if (Inst != this && Inst != null)
        {
            
            return;
        }
        else
        {
            Inst = this;

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DiasbleAction(bool disable)
    {

        disableAction = disable;
        if (!disableAction)
        {
            text.ShowText("");
        }

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
