using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Noise")]
public class PerlinNoiseData : NoiseData
{
    [Header("Noise")]
    public int MapSize = 200;
    public int Seed = 0;
    public int Scale = 1;
    public int Octave = 5;
    public float Persistence = 0.5f;
    public float Lacunarity = 1.5f;
    public Vector2 Offset = Vector2.zero;
    
    public override float[,] GenerateMap()
    {
        return PerlinNoise.GenerateNoiseMap(MapSize, MapSize, Seed, Scale, Octave, Persistence, Lacunarity, Offset);
    }
}
