using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Biome")]
public class BiomeData : ScriptableObject
{
    [Header("Noise")] public NoiseData Noise;

    [Header("Mesh")] 
    public int MapSize = 200;
    public int HeightMultiplier;
    public AnimationCurve heightCurve;
    public int LOD = 1;

    [Header("Materials")] 
    public List<BiomeMaterials> Materials;
}

[Serializable]
public struct BiomeMaterials
{
    public float zMax;
    public Material material;
}
