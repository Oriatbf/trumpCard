using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

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
        for(int i = curRelicCount; i < relics.Count; i++)
        {
            GameObject icon = Instantiate(relicImagePrefab,iconCanvas);
            icon.GetComponent<Image>().sprite = relics[i].relicIcon;
        }
        curRelicCount = relics.Count;
     
    }

    public void StartSkill()
    {
        Debug.Log("ff");
        if (relics.Count > 0)
        {
            foreach (RelicSO relic in relics)
            {
                foreach (RelicSO.RelicType relicType in relic.relicType)
                {
                    if (relicType.activeType == RelicSO.RelicType.ActiveType.Start)
                    {
                        relic.StartRelicActive(character.characterSO, relicType);
                    }
                }

            }
        }
       
    }

    public void StartRatioSkill()
    {
        if(relics.Count > 0)
        {
            foreach (RelicSO relic in relics)
            {
                foreach (RelicSO.RelicType relicType in relic.relicType)
                {
                    if (relicType.activeType == RelicSO.RelicType.ActiveType.Start)
                    {
                        relic.StartRatioRelicActive(character.characterSO, relicType);
                    }
                }

            }
        }
       
    }
   



    public void AttackSkill()
    {

    }


    public void ReloadSkill()
    {

    }
}
