using DG.Tweening;
using EasyTransition;
using Febucci.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VInspector;
using static RelicSO.RelicType;

public class LobbyNpc : MonoBehaviour
{
    public enum NpcType {bartender,slime,start }

    public NpcType npcType;


    [TextArea][SerializeField] string[] npcText;
    [SerializeField] Canvas textCanvas;
    
    [SerializeField] Vector2 size, pos;
    TypewriterByCharacter text;

    [SerializeField] LayerMask playerLayer;
    
    bool trig,startTrig;
    int textIndex;
    // Start is called before the first frame update
    void Start()
    {
        text = textCanvas.GetComponentInChildren<TypewriterByCharacter>();
        textCanvas.transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.OverlapBox(transform.position + (Vector3)pos, size, 0, playerLayer))
        {
            trig = true;
            if (npcType== NpcType.start && !startTrig )
            {
                startTrig = true;
                textCanvas.transform.DOScale(1, 0.5f).OnComplete(() => { DialogeText(); });
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.F1))
            {
                if (npcType == NpcType.start)
                {
                    DemoLoadScene.Inst.LoadScene("TestMapScene");
                    return;
                }
                if (textCanvas.transform.localScale.x != 1)
                    textCanvas.transform.DOScale(1, 0.5f).OnComplete(() => { DialogeText(); });
                else DialogeText();

                
             
            }
            
        }
        else if (trig)
        {
            startTrig = false;
            textCanvas.transform.DOScale(0, 0.5f);
            text.ShowText("");
            trig= false;
        }
    }

    [Button]
    public void DialogeText()
    {
        if(npcType == NpcType.slime)
            text.ShowText("<shake>" + npcText[textIndex]);
        else text.ShowText( npcText[textIndex]);
        textIndex++;
        if(textIndex == npcText.Length) textIndex= 0;

        
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + (Vector3)pos, size);
    }
   
}
