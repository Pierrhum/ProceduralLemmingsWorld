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

	/// <summary>
	/// Return the closest material of the biome, based on height 
	/// </summary>
	/// <param name="z">Height value</param>
	/// <returns>Biome material</returns>
	public Material getMaterial(float z)
	{
		BiomeMaterials highestBiomeMat = Materials[0];
		
		foreach(BiomeMaterials biomeMat in Materials)
			if (z < biomeMat.zMax && highestBiomeMat.zMax < biomeMat.zMax)
				highestBiomeMat = biomeMat;

		return highestBiomeMat.material;
	}
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