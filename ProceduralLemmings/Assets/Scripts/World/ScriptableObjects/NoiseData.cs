using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NoiseData : ScriptableObject
{
    // Type
    public abstract float[,] GenerateMap();
}
