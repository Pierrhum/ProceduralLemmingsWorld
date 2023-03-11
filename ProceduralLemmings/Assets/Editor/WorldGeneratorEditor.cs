using System;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor (typeof (WorldGenerator))]
public class WorldGeneratorEditor : Editor
{

	private WorldGenerator _wGen;
	private Transform _mapGenTransform;
	private Vector3 _currentTransformPosition;
	private Vector3 _currentTransformScale;

	void OnEnable() {
		_wGen = (WorldGenerator)target;
		_mapGenTransform = _wGen.transform;
	}

	void OnSceneGUI() {
		Handles.BeginGUI();
		GUILayout.BeginArea(new Rect(10, 155, 100, 100));
		Texture2D levelingColorMapTexture = DrawLevelingColorMap(_wGen.regionList, 100);
		EditorGUI.DrawPreviewTexture(new Rect(0, 0, 100, 100), levelingColorMapTexture);
		GUILayout.EndArea();
		Handles.EndGUI();
	}
	
	public override void OnInspectorGUI() {
		serializedObject.Update();
		
		GUILayout.Space(10);
		GUILayout.Label("DrawMode", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("drawMode"));
		
		GUILayout.Space(10);
		GUILayout.Label("World parameters", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("Scale"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("Offset"));
		
		GUILayout.Space(10);
		GUILayout.Label("Chunks", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("ChunksNumber"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("ChunkPrefab"));
		
		GUILayout.Space(10);
		GUILayout.Label("Noise", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("temperatureData"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("moistureData"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("regionList"));
		
		GUILayout.Space(10);
		GUILayout.Label("Update Params", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("autoUpdate"));

		if (serializedObject.ApplyModifiedProperties() 
		    || _wGen.transform.position != _currentTransformPosition
		    || _wGen.transform.lossyScale != _currentTransformScale) {
			if (_wGen.autoUpdate) {
				_wGen.OnGenerateWorld();
			}
		}
	
		if (GUILayout.Button ("Generate")) {
			_wGen.OnGenerateWorld();
		}
	
		_currentTransformPosition = _mapGenTransform.position;
		_currentTransformScale = _mapGenTransform.lossyScale;
	}
	
	private Texture2D DrawLevelingColorMap(BiomeData[] regionsData, int size) {
		int width = size, height = size;
        
		Texture2D texture = new Texture2D (width, height);
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;

		foreach (BiomeData regionData in regionsData) {
			foreach (Zone regionDataZone in regionData.zones) {
				int minX = (int)(width*regionDataZone.minMoisture);
				int maxX = (int)(width*regionDataZone.maxMoisture);
				if (maxX<minX) maxX = minX;
                    
				int minY = (int)(height*regionDataZone.minTemperature);
				int maxY = (int)(height*regionDataZone.maxTemperature);
				if (maxY<minY) maxY = minY;

				int blockX = width - maxX;
				int blockY = height - maxY;
				int blockWidth = width - minX - blockX;
				int blockHeight = height - minY - blockY;
				Color[] block = new Color[blockWidth * blockHeight];
				Array.Fill(block, regionData.color);
				texture.SetPixels(blockX, blockY, blockWidth, blockHeight, block);
			}
		}
		texture.Apply();
		return texture;
	}
	
}

