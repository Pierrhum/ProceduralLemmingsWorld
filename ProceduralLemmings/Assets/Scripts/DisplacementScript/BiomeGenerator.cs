using MyBox;
using UnityEngine;

public class BiomeGenerator : MonoBehaviour
{
    [SerializeField] DrawMode drawMode;
    private enum DrawMode { None, temperatureMap, MoistureMap, LevelingColorMap, BiomeColorMap, BiomeHeightMap};
    
    [SerializeField] private int size = 100;
    
    [Header("Noise")] 
    [SerializeField, DisplayInspector] private NoiseData temperatureNoise;
    [SerializeField, DisplayInspector] private NoiseData moistureNoise;
    
    [SerializeField, DisplayInspector] private BiomeData[] regionList;

    public bool autoUpdate;
    
    public void GenerateMap() {
        
        DisplayMapTexture display = FindObjectOfType<DisplayMapTexture>();
        if (drawMode == DrawMode.None) {
            display.ResetDisplay(size);
        }
        else if (drawMode == DrawMode.temperatureMap) {
            display.DrawNoiseMap(temperatureNoise.GenerateMap(), size);
        }
        else if (drawMode == DrawMode.MoistureMap) {
            display.DrawNoiseMap(moistureNoise.GenerateMap(), size);
        }
        else if (drawMode == DrawMode.LevelingColorMap) {
            display.DrawLevelingColorMap(regionList, size);
        }
        else if (drawMode == DrawMode.BiomeColorMap) {
            display.DrawColorBiomeZone(temperatureNoise.GenerateMap(), moistureNoise.GenerateMap(), regionList, size);
        }
        else if (drawMode == DrawMode.BiomeHeightMap) {
            display.DrawBiomeHeightMap(temperatureNoise.GenerateMap(), moistureNoise.GenerateMap(), regionList, size);
        }
    }

}
