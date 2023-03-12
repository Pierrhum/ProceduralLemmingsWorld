using System;
using UnityEngine;
using System.Collections.Generic;

public static class MeshGenerator {

	public static MeshData InitPlane(int mapSize, int subMeshCount=-1)
	{
		int width = mapSize + 1;
		int height = mapSize + 1;
		
		// Offset to center the plane
		Vector3 offset = new Vector3(-(width-1), 0, -(height-1)) / 2f;

		MeshData meshData = new MeshData (width, height, subMeshCount);
		int vertexIndex = 0;

		for (int y = 0; y < height; y ++) {
			for (int x = 0; x < width; x ++)
			{
				meshData.vertices [vertexIndex] = offset + new Vector3 (x, 0,y);
				meshData.uvs [vertexIndex] = new Vector2 (x / (float)width, y / (float)height);

				if (x < width - 1 && y < height - 1) {
					meshData.AddTriangle (vertexIndex + height, vertexIndex + 1, vertexIndex);
					meshData.AddTriangle (vertexIndex + height, vertexIndex + height + 1, vertexIndex + 1);
				}
				vertexIndex++;
			}
		}
		return meshData;
	}
	
	

	public static MeshData GenerateTerrainMesh(float[,] heightMap, int mapSize, float heightMultiplier, AnimationCurve heightCurve, int LOD) {
		int width = mapSize + 1;
		int height = mapSize + 1;
		
		// Offset to center the plane
		Vector3 offset = new Vector3(-(width-1), 0, -(height-1)) / 2f;

		MeshData meshData = new MeshData (width, height);
		int vertexIndex = 0;

		for (int y = 0; y < height; y ++) {
			for (int x = 0; x < width; x ++)
			{
				int hX = (int)(Mathf.InverseLerp(0, width, x) * NoiseData.NoiseSize);
				int hY = (int)(Mathf.InverseLerp(0, height, y) * NoiseData.NoiseSize);
				meshData.vertices [vertexIndex] = offset + new Vector3 (x, heightCurve.Evaluate(heightMap [hX, hY]) * heightMultiplier,y);
				meshData.uvs [vertexIndex] = new Vector2 (x / (float)width, y / (float)height);

				if (x < width - 1 && y < height - 1) {
					meshData.AddTriangle (vertexIndex + height, vertexIndex + 1, vertexIndex);
					meshData.AddTriangle (vertexIndex + height, vertexIndex + height + 1, vertexIndex + 1);
				}
				vertexIndex++;
			}
		}
		return meshData;
	}
}

public class MeshData {
	public Vector3[] vertices;
	public int[] triangles;
	private List<int>[] _submeshTriangles;
	public int[] biomeMap;
	public Vector2[] uvs;

	int triangleIndex;

	private int _meshWidth;
	private int _meshHeight;
	

	public MeshData(int meshWidth, int meshHeight, int subMeshCount=-1)
	{
		_meshWidth = meshWidth;
		_meshHeight = meshHeight;

		biomeMap = new int[meshWidth * meshHeight];
		vertices = new Vector3[meshWidth * meshHeight];
		uvs = new Vector2[meshWidth * meshHeight];
		triangles = new int[(meshWidth-1)*(meshHeight-1)*6];
		if (subMeshCount > 0)
		{
			_submeshTriangles = new List<int>[subMeshCount];
		
			for (var i = 0; i < subMeshCount; ++i)
			{
				_submeshTriangles[i] = new List<int>();
			}
		}
	}

	public void AddTriangle(int a, int b, int c) {
		triangles [triangleIndex] = a;
		triangles [triangleIndex + 1] = b;
		triangles [triangleIndex + 2] = c;
		triangleIndex += 3;
	}

	public void ApplyHeightMap(int x, int y, float heightValue, float heightMultiplier, AnimationCurve heightCurve)
	{
		Vector3 pos = vertices[y * _meshWidth + x];
		pos = new Vector3(pos.x, heightCurve.Evaluate(heightValue) * heightMultiplier, pos.z);
		vertices[y * _meshWidth + x] = pos;
	}
	
	public void SetSubMesh(int x, int y, int submesh)
	{
		int vertexIndex = y * _meshWidth + x;
		biomeMap[vertexIndex] = submesh;
		
		_submeshTriangles[submesh].Add(vertexIndex + _meshHeight); 
		_submeshTriangles[submesh].Add(vertexIndex + 1); 
		_submeshTriangles[submesh].Add(vertexIndex); 
		
		_submeshTriangles[submesh].Add(vertexIndex + _meshHeight); 
		_submeshTriangles[submesh].Add(vertexIndex + _meshHeight + 1); 
		_submeshTriangles[submesh].Add(vertexIndex + 1); 
	}

	public int GetBiomeAt(int x, int y)
	{
		int vertexIndex = y * _meshWidth + x;
		return biomeMap[vertexIndex];
	}

	public Mesh CreateMesh() {
		Mesh mesh = new Mesh ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		if (_submeshTriangles != null)
		{
			mesh.subMeshCount = _submeshTriangles.Length;
		
			for (var i = 0; i < _submeshTriangles.Length; ++i)
			{
				var submeshIndices = _submeshTriangles[i];
				var triangles = submeshIndices.ToArray();
				mesh.SetTriangles(triangles, i);
			}
			
		}
		
		mesh.RecalculateNormals ();
		return mesh;
	}
}