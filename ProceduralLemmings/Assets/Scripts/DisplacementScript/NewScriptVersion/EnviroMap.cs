
using UnityEngine;

public class EnviroMap : MonoBehaviour
{
    [SerializeField] private int size = 256;
    
    [Header("Noise")] 
    [SerializeField] private NoiseData temperatureNoise;
    [SerializeField] private NoiseData moistureNoise;

    [Header("BiomeLevel")] [SerializeField] private BiomeLevelMap _levelMap;
    
    private int[,] _biomeRegionMap { get; }

    public void GenerateMap() {
        if (_levelMap.IDBiomeLevelMap == null || _levelMap.IDBiomeLevelMap.Length == 0) return;
        int width = size, height = size;
        for (int x = 0; x < size; x++) {
            for (int y = 0; y < size; y++) {
                int nX = (int)(Mathf.InverseLerp(0, width, x) * NoiseData.NoiseSize);
                int nY = (int)(Mathf.InverseLerp(0, height, y) * NoiseData.NoiseSize);
                float currentTemperature = temperatureNoise.NoiseMap[nX, nY];
                float currentMoisture = moistureNoise.NoiseMap[nX, nY];
                
                
                
            }
        }
    }

    public void ShowOnTexture() {
        
    }
    
}