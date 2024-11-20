using DG.Tweening;
using EasyTransition;
using Febucci.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VInspector;
public enum NpcType {Npc,Player }
public class LobbyNpc : LobbyPlayer
{
   

    public NpcType npcType;


    [TextArea][SerializeField] string[] npcText;
    [SerializeField] private Canvas textCanvas;
    [SerializeField] private float detectRange;
    [SerializeField] private bool inDetect = false;
    
    TypewriterByCharacter text;
    int textIndex;
    
    // Start is called before the first frame update
    void Start()
    {
        text = textCanvas.GetComponentInChildren<TypewriterByCharacter>();
        textCanvas.transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    public override void Update()
    {
        if(!TutorialManager.Inst.isTutorialing && npcType == NpcType.Player)
            Move();

        if (npcType == NpcType.Npc)
        {
            DetectPlayer();
        }
        
    }

    public void DetectPlayer()
    {
        if (Vector3.Distance(NpcManager.Inst.GetPlayerNpc().transform.position, transform.position) < detectRange)
        {
            inDetect = true;
        }
        else inDetect = false;
        
        if(inDetect && Input.GetKeyDown(KeyCode.V) && npcType == NpcType.Npc)
            NpcInteract();
    }



    public void NpcInteract()
    {
        Debug.Log(gameObject.name);
        npcType = NpcType.Player;
        DOVirtual.DelayedCall(0.1f, () => NpcManager.Inst.SetPlayerNpc());
    }

    [Button]
    public void DialogeText()
    {
        if(npcType == NpcType.Player)
            text.ShowText("<shake>" + npcText[textIndex]);
        else text.ShowText( npcText[textIndex]);
        textIndex++;
        if(textIndex == npcText.Length) textIndex= 0;
  
    }

    public void TutorialDialogue()
    {
        textCanvas.transform.DOScale(1, 0.5f).OnComplete(() => 
        {
            text.ShowText(npcText[textIndex]);
            textIndex++;
            if (textIndex == npcText.Length) textIndex = 0;
        });

       // DOVirtual.DelayedCall(2f,)
       
    }

  
}
