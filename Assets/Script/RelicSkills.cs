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
        if(characterType == CharacterType.Enemy) relics = RelicManager.Inst.enemyRelic;
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
                BasicAbility basicRelic = item as BasicAbility;
                SpecialAbility specialRelic = item as SpecialAbility;
                InChantRelic inChantRelic= item as InChantRelic;
                if (basicRelic != null)
                {
                    foreach (BasicAbility.RelicType relicType in basicRelic.relicType)
                        basicRelic.Active(character.characterSO, relicType);
                }

                if (specialRelic != null)
                {
                    foreach (SpecialAbility.RelicType relicType in specialRelic.relicType)
                        specialRelic.Active(character.characterSO, relicType);
                }

                if (inChantRelic != null)
                {
                    foreach (InChantRelic.RelicType relicType in inChantRelic.relicType)
                        inChantRelic.Active(character.characterSO, relicType);
                }

            }

          
        }
       
    }

 
   



   
}
