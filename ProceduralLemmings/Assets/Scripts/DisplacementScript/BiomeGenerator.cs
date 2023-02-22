using MyBox;
using UnityEngine;

public class BiomeGenerator : MonoBehaviour
{
    [Header("Noise")] 
    [SerializeField, DisplayInspector] private NoiseData moistureNoise;
    [SerializeField, DisplayInspector] private NoiseData temperatureNoise;
    
    public TerrainType[] colorRegionList;

    public bool autoUpdate;
    
    public void GenerateMap() {
        
        DisplayMapTexture display = FindObjectOfType<DisplayMapTexture>();
        display.DrawNoiseMap (moistureNoise.GenerateMap());
        
    }

}
    
[System.Serializable]
public struct TerrainType {
    public string name;
    public float height;
    public float moisture;
    public float minDistance;
    public Color color;
    public GameObject vegetationPrefab;
}