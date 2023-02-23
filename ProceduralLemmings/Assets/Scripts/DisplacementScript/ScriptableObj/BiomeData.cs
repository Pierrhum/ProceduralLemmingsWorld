using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Biome")]
public class BiomeData : ScriptableObject
{
	public Color color;
	public Zone[] zones;
	
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

[Serializable]
public struct Zone {
	[Range(0.0f, 1.0f)] public float minTemperature;
	[Range(0.0f, 1.0f)] public float maxTemperature;
	[Range(0.0f, 1.0f)] public float minMoisture;
	[Range(0.0f, 1.0f)] public float maxMoisture;
}