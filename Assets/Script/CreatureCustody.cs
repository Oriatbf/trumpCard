using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CreatureCustody : MonoBehaviour
{
    [SerializeField] private List<Creature> creatures = new List<Creature>();
    [SerializeField] private List<float> multipliers = new List<float>();
    private Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    public void Init(Creature _creature)
    {
        Debug.Log("Init");
        creatures.Add(_creature);
    }
    
    public void Init(float _multipier)
    {
        multipliers.Add(_multipier);
    }

    public void summon()
    {
        foreach (var _creature in creatures)
        {
            if (multipliers.Count > 0)
            {
                foreach (var _mul in multipliers)
                {
                    _creature.UpgradeStat(_mul);
                }
            }

            Vector3 randomDir = Random.insideUnitCircle.normalized;
            Creature c = Instantiate(_creature,transform.position + randomDir ,Quaternion.identity);
            c.Init(character);
            c.transform.SetParent(null);
        }
    }

}
