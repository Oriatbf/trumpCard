using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicSkills : MonoBehaviour
{
    [SerializeField] List<RelicSO> relics= new List<RelicSO>();

    public enum CharacterType {Player ,Enemy };
    public CharacterType characterType;

    PlayerMove playerMove;
    private void Awake()
    {
        playerMove= GetComponent<PlayerMove>();
    }

    public void StartSkill()
    {
        foreach(RelicSO relic in relics)
        {
            foreach(RelicSO.RelicType relicType in relic.relicType)
            {
                if (relicType.activeType == RelicSO.RelicType.ActiveType.Start)
                {
                    relic.StartRelicActive(playerMove.characterSO,relicType);
                }
            }
           
        }      
    }

    public void StartRatioSkill()
    {
        foreach (RelicSO relic in relics)
        {
            foreach (RelicSO.RelicType relicType in relic.relicType)
            {
                if (relicType.activeType == RelicSO.RelicType.ActiveType.Start)
                {
                    relic.StartRatioRelicActive(playerMove.characterSO, relicType);
                }
            }

        }
    }
    public void MovingSkill()
    {
        foreach (RelicSO relic in relics)
        {
            foreach (RelicSO.RelicType relicType in relic.relicType)
            {
                if (relicType.activeType == RelicSO.RelicType.ActiveType.Moving)
                {
                    relic.Flooring(transform, relicType);
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
