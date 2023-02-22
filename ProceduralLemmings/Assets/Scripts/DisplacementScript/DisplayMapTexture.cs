using System;
using UnityEngine;

public class DisplayMapTexture : MonoBehaviour
{
    public Renderer textureRender;

    public void DrawNoiseMap(float[,] noiseMap) {
        textureRender.enabled = true;
        int width = noiseMap.GetLength (0);
        int height = noiseMap.GetLength (1);

        Texture2D texture = new Texture2D(width, height);

        Color[] colourMap = new Color[width * height];
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                colourMap [y * width + x] = Color.Lerp (Color.black, Color.white, noiseMap [x, y]);
            }
        }
        texture.SetPixels (colourMap);
        texture.Apply ();

        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3 (width, 1, height);
    }

    public void DrawLevelingColorMap(RegionData[] regionsData, int size) {
        textureRender.enabled = true;
        int width = size, height = size;
        Texture2D texture = new Texture2D (width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        
        SetupWater(texture, width, height);
        

        foreach (RegionData regionData in regionsData) {
            foreach (Zone regionDataZone in regionData.zones) {
                int x = (int)(width*regionDataZone.minMoisture);
                int y = (int)(height*regionDataZone.minTemperature);
                int blockWidth = width - x;
                int blockHeight = height - y;
                Color[] block = new Color[blockWidth * blockHeight];
                Array.Fill(block, regionData.color);
                texture.SetPixels(0, 0, blockWidth, blockHeight, block);
            }
        }
        texture.Apply();
        
        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3 (width, 1, height);
    }

    public void DrawColorBiomeZone(float[,] temperatureMap, float[,] moistureMap, RegionData[] regionsData, int size) {
        textureRender.enabled = true;
        Color[] biomeMap = FillWater(size, size);
        for (int x = 0; x < size; x++) {
            for (int y = 0; y < size; y++) {
                float currentTemperature = temperatureMap[x, y];
                float currentMoisture = moistureMap[x, y];

                foreach (RegionData regionData in regionsData) {
                    foreach (Zone regionDataZone in regionData.zones) {
                        if (currentTemperature >= regionDataZone.minTemperature && currentMoisture >= regionDataZone.minMoisture) {
                            biomeMap[y * size + x] = regionData.color;
                        }
                    }
                }
            }
        }

        DrawTexture(TextureFromColorMap(biomeMap, size, size));
    }

    private Color[] FillWater(int width, int height) {
        Color[] block = new Color[width * height];
        Array.Fill(block, Color.blue);
        return block;
    }
    
    private void SetupWater(Texture2D texture, int width, int height) {
        //setup water
        texture.SetPixels(0, 0, width, height, FillWater(width, height));
        texture.Apply();
    }
    
    public Texture2D TextureFromColorMap(Color[] colorMap, int width, int height) {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorMap);
        texture.Apply();
        return texture;
    }
    
    public void DrawTexture(Texture2D texture) {
        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void ResetDisplay() {
        textureRender.enabled = false;
    }
}
