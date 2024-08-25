using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class GambleManager 
{
    public static int GambleIndex()
    {
        int ramdomIndex = Random.Range(0,12);
        return ramdomIndex;
    }

}
