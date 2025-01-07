using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
        creatures.Add(_creature);
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

            Creature c = Instantiate(_creature, transform);
            c.Init(character);
            c.transform.SetParent(null);
        }
    }

}
