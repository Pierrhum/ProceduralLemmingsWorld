using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [NonSerialized] public MeshData meshData;
    [NonSerialized] public Mesh mesh;
    private int Size;
    public void CreateMesh(int ChunkSize, int subMeshCount=-1)
    {
        Size = ChunkSize;
        meshData = MeshGenerator.InitPlane(ChunkSize, subMeshCount);
        mesh = meshData.CreateMesh();
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.sharedMesh = mesh;
    }

    public void UpdateMesh(Mesh _mesh, List<Material> materials=null)
    {
        mesh = _mesh;
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;
        if (materials != null)
            GetComponent<MeshRenderer>().materials = materials.ToArray();
    }

    public int GetBiomeAt(int x, int y)
    {
        return meshData.GetBiomeAt(x,y);
    }
}
