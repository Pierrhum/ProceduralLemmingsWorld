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
    [SerializeField, DisplayInspector] private NoiseData temperatureData;
    [SerializeField, DisplayInspector] private NoiseData moistureData;
    [SerializeField, DisplayInspector] private BiomeData[] regionList;

    private float[,] temperatureNoise;
    private float[,] moistureNoise;
    
    public bool autoUpdate;

    private void Awake()
    {
        temperatureNoise = temperatureData.GenerateMap(Vector2.zero);
        moistureNoise = moistureData.GenerateMap(Vector2.zero);
    }

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
            //temperatureNoise.Pos = Offset;
            display.DrawNoiseMap(temperatureNoise, size);
        }
        else if (drawMode == DrawMode.MoistureMap) {
           // moistureNoise.Pos = Offset;
            display.DrawNoiseMap(moistureNoise, size);
        }
        else if (drawMode == DrawMode.LevelingColorMap) {
            display.DrawLevelingColorMap(regionList, size);
        }
        else if (drawMode == DrawMode.BiomeColorMap) {
         //   temperatureNoise.Pos = Offset;
           // moistureNoise.Pos = Offset;
            display.DrawColorBiomeZone(temperatureNoise, moistureNoise, regionList, size);
        }
        else if (drawMode == DrawMode.BiomeHeightMap) {
           // temperatureNoise.Pos = Offset;
           // moistureNoise.Pos = Offset;
            display.DrawBiomeHeightMap(temperatureNoise, moistureNoise, regionList, size);
        }
    }

}
