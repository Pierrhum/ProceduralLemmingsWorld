using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [Header("DrawMode")] 
    [SerializeField] private DrawMode drawMode;
    
    [Header("World parameters")]
    [SerializeField] private int Seed = 0;
    [SerializeField] private int Scale = 200;
    [SerializeField] private Vector2 Offset = Vector2.zero;
    
    [Header("Chunks")] 
    [SerializeField] private int ChunksNumber;
    [SerializeField] private GameObject ChunkPrefab;
    
    [Header("Noise")] 
    [SerializeField, DisplayInspector] private NoiseData temperatureData;
    [SerializeField, DisplayInspector] private NoiseData moistureData;
    [SerializeField, DisplayInspector] private BiomeData[] regionList;
    
    private enum DrawMode { None, temperatureMap, MoistureMap, LevelingColorMap, BiomeColorMap, BiomeHeightMap};
    private float[,] temperatureNoise;
    private float[,] moistureNoise;

    private List<Chunk> Chunks;
    private void OnValidate()
    {
        temperatureData.Seed = Seed;
        temperatureData.Scale = Scale;
        temperatureData.Offset = Offset;
        moistureData.Seed = Seed;
        moistureData.Scale = Scale;
        moistureData.Offset = Offset;

        foreach (Transform child in transform)
            StartCoroutine(DestroyOnValidate(child.gameObject));
        
        Chunks.Clear();
        GenerateChunks();
    }
    
    /// <summary>
    /// Call this coroutine to destroy a GameObject in the OnValidate method
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    private IEnumerator DestroyOnValidate(GameObject go)
    {
        yield return null;
        DestroyImmediate(go);
    }

    private void GenerateChunks()
    {
        for (int x = 0; x < ChunksNumber; x++)
            for (int y = 0; y < ChunksNumber; y++)
            {
                Vector3 ChunkPos = new Vector3((x * Scale) - (ChunksNumber * Scale) / 2, 0, (y * Scale) - (ChunksNumber * Scale) / 2);
                Vector2 NoisePos = new Vector2(x * (NoiseData.NoiseSize - 1), y * (NoiseData.NoiseSize - 1));
                // Création/Position du mesh 
                GameObject go = Instantiate(ChunkPrefab, ChunkPos, Quaternion.identity, transform);
                Chunk chunk = go.GetComponent<Chunk>();
                chunk.CreateMesh(Scale);
                // Génération/Position du bruit
                temperatureNoise = temperatureData.GenerateMap(NoisePos);
                moistureNoise = moistureData.GenerateMap(NoisePos);
                DrawChunk(chunk);
                
                Chunks.Add(chunk);
            }
    }

    private void DrawChunk(Chunk chunk)
    {
        DisplayMapTexture display = chunk.GetComponent<DisplayMapTexture>();
        if (drawMode == DrawMode.None) {
            display.ResetDisplay(Scale);
        }
        else if (drawMode == DrawMode.temperatureMap) {
            display.DrawNoiseMap(temperatureNoise, Scale);
        }
        else if (drawMode == DrawMode.MoistureMap) {
            display.DrawNoiseMap(moistureNoise, Scale);
        }
        else if (drawMode == DrawMode.LevelingColorMap) {
            display.DrawLevelingColorMap(regionList, Scale);
        }
        else if (drawMode == DrawMode.BiomeColorMap) {
            display.DrawColorBiomeZone(temperatureNoise, moistureNoise, regionList, Scale);
        }
        else if (drawMode == DrawMode.BiomeHeightMap) {
            display.DrawBiomeHeightMap(temperatureNoise, moistureNoise, regionList, Scale);
        }
        
    }
}
