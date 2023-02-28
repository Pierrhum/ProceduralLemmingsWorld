using System;
using UnityEngine;

public class DisplayMapTexture : MonoBehaviour
{
    public void DrawNoiseMap(float[,] noiseMap, int size) {
        MeshData meshData = MeshGenerator.InitPlane(size);
        int width = size + 1;
        int height = size + 1;

        Color[] colourMap = new Color[width * height];
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                int cX = (int)(Mathf.InverseLerp(0, width, x) * noiseMap.GetLength(0));
                int cY = (int)(Mathf.InverseLerp(0, height, y) * noiseMap.GetLength(1));
                colourMap [y * width + x] = Color.Lerp (Color.black, Color.white, noiseMap [cX, cY]);
            }
        }
        Mesh mesh = meshData.CreateMesh();
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;
        DrawTexture(TextureFromColorMap(colourMap, width, height));
    }

    public void DrawLevelingColorMap(BiomeData[] regionsData, int size) {
        MeshData meshData = MeshGenerator.InitPlane(size);
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
        
        Mesh mesh = meshData.CreateMesh();
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    public void DrawColorBiomeZone(float[,] temperatureMap, float[,] moistureMap, BiomeData[] regionsData, int size) {
        MeshData meshData = MeshGenerator.InitPlane(size);
        int width = size, height = size;
        Color[] biomeMap = new Color[width * height];
        
        for (int x = 0; x < size; x++) {
            for (int y = 0; y < size; y++) {
                int nX = (int)(Mathf.InverseLerp(0, width, x) * NoiseData.NoiseSize);
                int nY = (int)(Mathf.InverseLerp(0, height, y) * NoiseData.NoiseSize);
                float currentTemperature = temperatureMap[nX, nY];
                float currentMoisture = moistureMap[nX, nY];

                foreach (BiomeData biomeData in regionsData) {
                    foreach (Zone regionDataZone in biomeData.zones) {
                        if (currentTemperature >= regionDataZone.minTemperature && 
                            currentMoisture >= regionDataZone.minMoisture && 
                            currentTemperature <= regionDataZone.maxTemperature && 
                            currentMoisture <= regionDataZone.maxMoisture) {
                            biomeMap[y * size + x] = biomeData.color;
                        }
                    }
                }
            }
        }
        
        Mesh mesh = meshData.CreateMesh();
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;
        DrawTexture(TextureFromColorMap(biomeMap, size, size));
    }

    public void DrawBiomeHeightMap(float[,] temperatureMap, float[,] moistureMap, BiomeData[] regionsData, int size) {
        MeshData meshData = MeshGenerator.InitPlane(size);
        int width = size, height = size;
        Color[] biomeMap = new Color[width * height];

        
        foreach (BiomeData biomeData in regionsData) {
            if (biomeData.Noise == null) continue;
            float[,] heightMap = biomeData.Noise.GenerateMap(Vector2.zero);
            foreach (Zone regionDataZone in biomeData.zones) {
                for (int x = 0; x < size; x++) {
                    for (int y = 0; y < size; y++) {
                        //temperature value
                        int nX = (int)(Mathf.InverseLerp(0, width, x) * NoiseData.NoiseSize);
                        int nY = (int)(Mathf.InverseLerp(0, height, y) * NoiseData.NoiseSize);
                        float currentTemperature = temperatureMap[nX, nY];
                        float currentMoisture = moistureMap[nX, nY];
                        if (currentTemperature >= regionDataZone.minTemperature && 
                            currentMoisture >= regionDataZone.minMoisture && 
                            currentTemperature <= regionDataZone.maxTemperature && 
                            currentMoisture <= regionDataZone.maxMoisture) {
                            biomeMap[y * size + x] = Color.Lerp(Color.black, Color.white, heightMap[nX, nY]) ;
                            meshData.ApplyHeightMap(x,y,heightMap[nX, nY],  biomeData.HeightMultiplier, biomeData.heightCurve);
                        }
                    }
                }
            }
        }

        Mesh mesh = meshData.CreateMesh();
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        meshFilter.sharedMesh = mesh;
        meshCollider.sharedMesh = mesh;
        
        DrawTexture(TextureFromColorMap(biomeMap, size, size));
        
    }

    public void ResetDisplay(int size) {
        Color[] reset = new Color[size * size];
        Array.Fill(reset, Color.white);
        DrawTexture(TextureFromColorMap(reset, size, size));
    }
    
    private Texture2D TextureFromColorMap(Color[] colorMap, int width, int height) {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorMap);
        texture.Apply();
        return texture;
    }
    
    private void DrawTexture(Texture2D texture) {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        Material instance = new Material(renderer.sharedMaterial);
        instance.mainTexture = texture;
        renderer.sharedMaterial = instance;
    }

}
