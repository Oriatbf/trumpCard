using DG.Tweening;
using EasyTransition;
using Febucci.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayableCharacterData;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using VInspector;

public class LobbyNpc : LobbyInteraction,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    [TextArea][SerializeField] string[] npcText;
    public int npcId;
    [SerializeField] private Canvas textCanvas;
    private PlayableNpcManager playableNpcManager;
    
 
    TypewriterByCharacter text;
    int textIndex;
    
    public void SetNpcManager(PlayableNpcManager playableNpcManager)
    {
        this.playableNpcManager = playableNpcManager;
    }

    
    // OnGamble is called before the first frame update
    void Start()
    {
        
        text = textCanvas.GetComponentInChildren<TypewriterByCharacter>();
        textCanvas.transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    public void Update()
    {
        if ( npcType == NpcType.Player)
            Move();

        if (npcType == NpcType.Player)
        {
            DetectNpc();
        }
        

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!playableNpcManager.characterSelected)
        {
            Debug.Log($"{gameObject.name} 클릭됨!");
            playableNpcManager.TurningNpc(npcId);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!playableNpcManager.characterSelected)
        {
            transform.DOScale(new Vector3(1.2f, 1.2f), 0.15f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!playableNpcManager.characterSelected)
        {
            transform.DOScale(new Vector3(1f, 1f),0.15f);
        }
    }
}
