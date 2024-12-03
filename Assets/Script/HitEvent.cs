using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEvent : MonoBehaviour
{
    Character character;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponentInParent<Character>();

    }

    public void hitEvent()
    {
        character.MeleeDamage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
