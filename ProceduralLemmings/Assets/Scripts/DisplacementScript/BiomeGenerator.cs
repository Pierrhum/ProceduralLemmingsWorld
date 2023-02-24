using System;
using MyBox;
using UnityEngine;

public class BiomeGenerator : MonoBehaviour
{
    [SerializeField] DrawMode drawMode;
    private enum DrawMode { None, temperatureMap, MoistureMap, LevelingColorMap, BiomeColorMap, BiomeHeightMap};
    
    [SerializeField] private int size = 100;
    public Vector2 Offset = Vector2.zero;
    
    [Header("Noise")] 
    [SerializeField, DisplayInspector] private NoiseData temperatureNoise;
    [SerializeField, DisplayInspector] private NoiseData moistureNoise;
    
    [SerializeField, DisplayInspector] private BiomeData[] regionList;

    public bool autoUpdate;

    private void OnValidate()
    {
        if(autoUpdate)
            GenerateMap();
    }

    public void GenerateMap() {

        DisplayMapTexture display = GetComponent<DisplayMapTexture>();
        if (drawMode == DrawMode.None) {
            display.ResetDisplay(size);
        }
        else if (drawMode == DrawMode.temperatureMap) {
            temperatureNoise.Pos = Offset;
            display.DrawNoiseMap(temperatureNoise.GenerateMap(), size);
        }
        else if (drawMode == DrawMode.MoistureMap) {
            moistureNoise.Pos = Offset;
            display.DrawNoiseMap(moistureNoise.GenerateMap(), size);
        }
        else if (drawMode == DrawMode.LevelingColorMap) {
            display.DrawLevelingColorMap(regionList, size);
        }
        else if (drawMode == DrawMode.BiomeColorMap) {
            temperatureNoise.Pos = Offset;
            moistureNoise.Pos = Offset;
            display.DrawColorBiomeZone(temperatureNoise.GenerateMap(), moistureNoise.GenerateMap(), regionList, size);
        }
        else if (drawMode == DrawMode.BiomeHeightMap) {
            temperatureNoise.Pos = Offset;
            moistureNoise.Pos = Offset;
            display.DrawBiomeHeightMap(temperatureNoise.GenerateMap(), moistureNoise.GenerateMap(), regionList, size);
        }
    }

}
