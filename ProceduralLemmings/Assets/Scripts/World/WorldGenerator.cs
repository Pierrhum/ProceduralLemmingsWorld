using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public enum DrawMode { None, temperatureMap, MoistureMap, BiomeColorMap, BiomeHeightMap};

public class WorldGenerator : MonoBehaviour
{
    //[Header("DrawMode")] 
    [SerializeField] [HideInInspector] private DrawMode drawMode;

    //[Header("World parameters")]
    [SerializeField] [HideInInspector] private int Scale = 200;
    [SerializeField] [HideInInspector] private Vector2 Offset = Vector2.zero;
    
    //[Header("Chunks")] 
    [SerializeField] [HideInInspector] private int ChunksNumber;
    [SerializeField] [HideInInspector] private GameObject ChunkPrefab;
    
    //[Header("Noise")] 
    [SerializeField, DisplayInspector] [HideInInspector] private NoiseData temperatureData;
    [SerializeField, DisplayInspector] [HideInInspector] private NoiseData moistureData;
    [SerializeField, DisplayInspector] [HideInInspector] public BiomeData[] regionList;
    
    [SerializeField] public bool autoUpdate;
    
    private float[,] temperatureNoise;
    private float[,] moistureNoise;

    private List<Chunk> Chunks = new List<Chunk>();
    
    private void OnValidate() {
        if (autoUpdate) {
            OnGenerateWorld();
        }
    }

    public void OnGenerateWorld(){
        temperatureData.Scale = Scale;
        temperatureData.Offset = Offset;
        moistureData.Scale = Scale;
        moistureData.Offset = Offset;

        foreach (Transform child in transform)
            StartCoroutine(DestroyOnValidate(child.gameObject));
        if(Chunks.Count > 0)
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
        Chunks = new List<Chunk>();
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
        DisplayChunkTexture display = chunk.GetComponent<DisplayChunkTexture>();
        if (drawMode == DrawMode.None) {
            display.ResetDisplay(Scale);
        }
        else if (drawMode == DrawMode.temperatureMap) {
            display.DrawNoiseMap(temperatureNoise, Scale);
        }
        else if (drawMode == DrawMode.MoistureMap) {
            display.DrawNoiseMap(moistureNoise, Scale);
        }
        else if (drawMode == DrawMode.BiomeColorMap) {
            display.DrawColorBiomeZone(temperatureNoise, moistureNoise, regionList, Scale);
        }
        else if (drawMode == DrawMode.BiomeHeightMap) {
            display.DrawBiomeHeightMap(temperatureNoise, moistureNoise, regionList, Scale);
        }
        
    }
}
