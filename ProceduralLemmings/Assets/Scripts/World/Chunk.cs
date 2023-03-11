using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [NonSerialized] public MeshData meshData;
    [NonSerialized] public Mesh mesh;
    private int Size;
    public void CreateMesh(int ChunkSize)
    {
        Size = ChunkSize;
        meshData = MeshGenerator.InitPlane(ChunkSize);
        mesh = meshData.CreateMesh();
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    public void Build(float[,] temperatureMap, float[,] moistureMap, BiomeData[] regionsData)
    {
        foreach (BiomeData biomeData in regionsData) {
            if (biomeData.Noise == null) continue;
            float[,] heightMap = biomeData.Noise.GenerateMap(Vector2.zero);
            foreach (Zone regionDataZone in biomeData.zones) {
                for (int x = 0; x < Size; x++) {
                    for (int y = 0; y < Size; y++) {
                        //temperature value
                        int nX = (int)(Mathf.InverseLerp(0, Size, x) * NoiseData.NoiseSize);
                        int nY = (int)(Mathf.InverseLerp(0, Size, y) * NoiseData.NoiseSize);
                        float currentTemperature = temperatureMap[nX, nY];
                        float currentMoisture = moistureMap[nX, nY];
                        if (currentTemperature >= regionDataZone.minTemperature && 
                            currentMoisture >= regionDataZone.minMoisture && 
                            currentTemperature <= regionDataZone.maxTemperature && 
                            currentMoisture <= regionDataZone.maxMoisture) {
                            //biomeMap[y * Size + x] = Color.Lerp(Color.black, Color.white, heightMap[nX, nY]) ;
                            meshData.ApplyHeightMap(x,y,heightMap[nX, nY],  biomeData.HeightMultiplier, biomeData.heightCurve);
                        }
                    }
                }
            }
        }
    }

    public void UpdateMesh(Mesh _mesh)
    {
        mesh = _mesh;
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;
    }
    
    private Texture2D BuildTexture(float[,] heightMap, bool noise=true) {
        int width = heightMap.GetLength (0);
        int height = heightMap.GetLength (1);
        Color[] colorMap = new Color[height * width];
        int matIndex = 0;
        
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                int colorIndex = y * width + x;
                float heightValue = heightMap[x, y];
                if(noise)
                    colorMap [colorIndex] = Color.Lerp (Color.black, Color.white, heightValue);
                else
                {
                    // Color map according to texture
                   // if (heightValue / 255.0f > region.Materials[matIndex].zMax) matIndex++;
                    
                } 
                    
            }
        }
        // create a new texture and set its pixel colors
        Texture2D tileTexture = new Texture2D (width, height);
        tileTexture.wrapMode = TextureWrapMode.Clamp;
        tileTexture.SetPixels (colorMap);
        tileTexture.Apply ();
        return tileTexture;
    }
}
