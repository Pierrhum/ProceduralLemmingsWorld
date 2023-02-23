using UnityEngine;

[CreateAssetMenu(menuName = "Data/Region")]
public class RegionData : ScriptableObject
{
	public Color color;
	public Zone[] zones;
}

[System.Serializable]
public struct Zone {
	[Range(0.0f, 1.0f)] public float minTemperature;
	[Range(0.0f, 1.0f)] public float maxTemperature;
	[Range(0.0f, 1.0f)] public float minMoisture;
	[Range(0.0f, 1.0f)] public float maxMoisture;
}