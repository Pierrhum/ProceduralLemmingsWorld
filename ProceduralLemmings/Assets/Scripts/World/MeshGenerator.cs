using System;
using UnityEngine;
using System.Collections.Generic;

public static class MeshGenerator {

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
				int hX = (int)(Mathf.InverseLerp(0, width, x) * heightMap.GetLength(0));
				int hY = (int)(Mathf.InverseLerp(0, height, y) * heightMap.GetLength(1));
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
	public Vector2[] uvs;

	int triangleIndex;

	public MeshData(int meshWidth, int meshHeight) {
		vertices = new Vector3[meshWidth * meshHeight];
		uvs = new Vector2[meshWidth * meshHeight];
		triangles = new int[(meshWidth-1)*(meshHeight-1)*6];
	}

	public void AddTriangle(int a, int b, int c) {
		triangles [triangleIndex] = a;
		triangles [triangleIndex + 1] = b;
		triangles [triangleIndex + 2] = c;
		triangleIndex += 3;
	}

	public Mesh CreateMesh() {
		Mesh mesh = new Mesh ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.RecalculateNormals ();
		return mesh;
	}
}