using System;
using UnityEngine;

public class DisplayMapTexture : MonoBehaviour
{
    public Renderer textureRender;

    public void DrawNoiseMap(float[,] noiseMap, int size) {
        textureRender.enabled = true;
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
        
        DrawTexture(TextureFromColorMap(colourMap, width, height));
    }

    public void DrawLevelingColorMap(RegionData[] regionsData, int size) {
        textureRender.enabled = true;
        int width = size, height = size;
        
        Texture2D texture = new Texture2D (width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        foreach (RegionData regionData in regionsData) {
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
        
        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3 (width, 1, height);
    }

    public void DrawColorBiomeZone(float[,] temperatureMap, float[,] moistureMap, RegionData[] regionsData, int size) {
        textureRender.enabled = true;
        int width = size, height = size;
        Color[] biomeMap = new Color[width * height];
        
        for (int x = 0; x < size; x++) {
            for (int y = 0; y < size; y++) {
                
                int tX = (int)(Mathf.InverseLerp(0, width, x) * temperatureMap.GetLength(0));
                int tY = (int)(Mathf.InverseLerp(0, height, y) * temperatureMap.GetLength(1));
                float currentTemperature = temperatureMap[tX, tY];
                
                int mX = (int)(Mathf.InverseLerp(0, width, x) * temperatureMap.GetLength(0));
                int mY = (int)(Mathf.InverseLerp(0, height, y) * temperatureMap.GetLength(1));
                float currentMoisture = moistureMap[mX, mY];

                foreach (RegionData regionData in regionsData) {
                    foreach (Zone regionDataZone in regionData.zones) {
                        if (currentTemperature >= regionDataZone.minTemperature && 
                            currentMoisture >= regionDataZone.minMoisture && 
                            currentTemperature <= regionDataZone.maxTemperature && 
                            currentMoisture <= regionDataZone.maxMoisture) {
                            biomeMap[y * size + x] = regionData.color;
                        }
                    }
                }
            }
        }

        DrawTexture(TextureFromColorMap(biomeMap, size, size));
    }

    
    public void ResetDisplay(int size) {
        textureRender.enabled = true;
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
        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

}
