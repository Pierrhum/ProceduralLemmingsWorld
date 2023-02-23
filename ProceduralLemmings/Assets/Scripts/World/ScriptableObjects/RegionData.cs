using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Region")]
public class RegionData : ScriptableObject
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

