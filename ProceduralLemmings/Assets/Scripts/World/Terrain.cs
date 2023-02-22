using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    [SerializeField, DisplayInspector] private BiomeData biome;
    [SerializeField] private bool ApplyNoiseTexture;
    [SerializeField] private bool GenerateTerrain;
    
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        float[,] heightMap = biome.Noise.GenerateMap();
        Mesh mesh = MeshGenerator.GenerateTerrainMesh(heightMap, biome.MapSize, biome.HeightMultiplier, biome.heightCurve, biome.LOD).CreateMesh();

        Texture2D tileTexture = null;
        if (ApplyNoiseTexture)
        {
            tileTexture = BuildTexture(heightMap);
            meshRenderer.sharedMaterial.mainTexture = tileTexture;
        } else if (biome.Materials.Count > 0)
        {
            
        }
        else meshRenderer.sharedMaterial.mainTexture = tileTexture;
        meshFilter.mesh = mesh;
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
                    if (heightValue / 255.0f > biome.Materials[matIndex].zMax) matIndex++;
                    
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

    private void OnDrawGizmos()
    {
        if (GenerateTerrain)
        {
            Generate();
            GenerateTerrain = false;
        }
    }
}
