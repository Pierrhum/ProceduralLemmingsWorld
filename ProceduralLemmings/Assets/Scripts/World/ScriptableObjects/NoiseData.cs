using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NoiseData : ScriptableObject
{
    [Header("Noise")] 
    public int Seed = 0;
    public float Scale = 1f;
    public Vector2 Offset = Vector2.zero;

    // 256 * 256
    public static int NoiseSize = 256;

    // To change only if octaves or 
    protected float minNoiseHeight = float.MaxValue;
    protected float maxNoiseHeight = float.MinValue;
    public abstract float[,] GenerateMap(Vector2 Pos);

    public void ResetNoiseBounds() {
        minNoiseHeight = float.MaxValue;
        maxNoiseHeight = float.MinValue;
    }

    protected float[,] NormalizeNoise(float[,] noiseMap)
    {
        for (int y = 0; y < NoiseSize; y++) 
            for (int x = 0; x < NoiseSize; x++) 
                noiseMap [x, y] = Mathf.InverseLerp (minNoiseHeight, maxNoiseHeight, noiseMap [x, y]);
        return noiseMap;
    }
}
