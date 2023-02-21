using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Biome/Data")]
public class BiomeData : ScriptableObject
{
    [Header("Noise")] 
    public int MapSize = 200;
    public int Seed = 0;
    public int Scale = 1;
    public int Octave = 5;
    public float Persistence = 0.5f;
    public float Lacunarity = 1.5f;
    public Vector2 Offset = Vector2.zero;

    [Header("Mesh")] 
    public int HeightMultiplier;
    public AnimationCurve heightCurve;
    public int LOD;
}
