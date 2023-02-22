using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NoiseData : ScriptableObject
{
    [Header("Noise")]
    public int Seed = 0;
    public float Scale = 1f;

    // 256 * 256
    protected int NoiseSize = 256;
    // Type
    public abstract float[,] GenerateMap();
}
