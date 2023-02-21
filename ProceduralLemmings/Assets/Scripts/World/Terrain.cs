using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    [SerializeField, DisplayInspector] private BiomeData biome;
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

        float[,] heightMap = biome.Noise.GenerateMap();
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
