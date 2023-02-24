using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] private int Chunks;
    [SerializeField] private float ChunkSize;
    private BiomeGenerator Generator;

    private void OnValidate()
    {
        throw new NotImplementedException();
    }

    private void GenerateChunks()
    {
        
    }
}
