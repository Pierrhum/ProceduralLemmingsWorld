using MyBox;
using UnityEngine;

public class BiomeGenerator : MonoBehaviour
{
    [SerializeField] DrawMode drawMode;
    private enum DrawMode { None, temperatureMap, MoistureMap, LevelingColorMap, BiomeColorMap};
    
    [Header("Noise")] 
    [SerializeField, DisplayInspector] private NoiseData moistureNoise;
    [SerializeField, DisplayInspector] private NoiseData temperatureNoise;
    
    [SerializeField, DisplayInspector] private RegionData[] regionList;

    public bool autoUpdate;
    
    public void GenerateMap() {
        
        DisplayMapTexture display = FindObjectOfType<DisplayMapTexture>();
        if (drawMode == DrawMode.None) {
            display.ResetDisplay();
        }
        else if (drawMode == DrawMode.temperatureMap) {
            display.DrawNoiseMap(moistureNoise.GenerateMap());
        }
        else if (drawMode == DrawMode.MoistureMap) {
            display.DrawNoiseMap(temperatureNoise.GenerateMap());
        }
        else if (drawMode == DrawMode.LevelingColorMap) {
            display.DrawLevelingColorMap(regionList, 100);
        }
        else if (drawMode == DrawMode.BiomeColorMap) {
            display.DrawColorBiomeZone(temperatureNoise.GenerateMap(), moistureNoise.GenerateMap(), regionList, 100);
        }
    }

}
