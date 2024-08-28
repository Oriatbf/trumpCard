using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using VInspector;
using static BasicAbility;

public class RelicSkills : MonoBehaviour
{
    [Tab ("Relic")]
    public List<RelicSO> relics= new List<RelicSO>();

    public enum CharacterType {Player ,Enemy };
    
    [Tab ("Player")]
    public CharacterType characterType;
    [SerializeField] GameObject relicImagePrefab;
    [SerializeField] Transform iconCanvas;
    private int curRelicCount;


    Character character;
    private void Awake()
    {
        character= GetComponent<Character>();
        if (characterType == CharacterType.Player) relics = RelicManager.Inst.playerRelic;
    }
    [Button]
    public void relicDebug()
    {
        Debug.Log(RelicManager.Inst.playerRelic.Count);
       relics = RelicManager.Inst.playerRelic;
        Debug.Log(relics.Count);
    }

    public void SetRelicIcon() 
    {
        /*
        for(int i = curRelicCount; i < relics.Count; i++)
        {
            GameObject icon = Instantiate(relicImagePrefab,iconCanvas);
            icon.GetComponent<Image>().sprite = relics[i].relicIcon;
        }
        curRelicCount = relics.Count;
        */
     
    }

    
    public void StartSkill()
    {
        if (relics.Count > 0)
        {
            foreach (var item in relics)
            {
                BasicAbility relic = item as BasicAbility;
                if (relic != null)
                {
                    foreach (BasicAbility.RelicType relicType in relic.relicType)
                        relic.Active(character.characterSO, relicType);
                }
              
            }

            foreach (var item in relics)
            {
                SpecialAbility relic = item as SpecialAbility;
                if (relic != null)
                {
                    foreach (SpecialAbility.RelicType relicType in relic.relicType)
                        relic.Active(character.characterSO, relicType);
                }
               
            }
        }
       
    }

 
   



   
}
