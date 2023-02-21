using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    [SerializeField] private BiomeData biome;
    [SerializeField] private bool GenerateTerrain;
    
    [SerializeField] private AnimationCurve heightCurve;
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        float[,] heightMap = Noise.GenerateNoiseMap(biome.MapSize, biome.MapSize, biome.Seed, biome.Scale, biome.Octave, biome.Persistence, biome.Lacunarity, biome.Offset);
        Mesh mesh = MeshGenerator.GenerateTerrainMesh(heightMap, biome.HeightMultiplier, heightCurve, biome.LOD).CreateMesh();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    private void OnDrawGizmos()
    {
        if (GenerateTerrain)
        {
            meshFilter = GetComponent<MeshFilter>();
            meshCollider = GetComponent<MeshCollider>();
            Generate();
            GenerateTerrain = false;
        }
    }
}
