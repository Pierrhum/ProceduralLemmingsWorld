using UnityEngine;

[CreateAssetMenu(menuName = "Data/Biome")]
public class BiomeLevelMap : ScriptableObject
{
    [SerializeField] private int size = 100;
    [SerializeField] public BiomeData[] _biomeDataList;
    
    public int[,] IDBiomeLevelMap { get; }
    
    public BiomeLevelMap() {
        IDBiomeLevelMap = new int[size, size];
    }

    public void GenerateMap() {
        int width = size, height = size;
        
        foreach (BiomeData biomeData in _biomeDataList) {
            if (biomeData.Noise == null) continue;
            foreach (RegionLevel regionLevel in biomeData.zones) {
                int minX = (int) (width * regionLevel.minMoisture);
                int maxX = (int) (width * regionLevel.maxMoisture);
                if (maxX < minX) maxX = minX;

                int minY = (int) (height * regionLevel.minTemperature);
                int maxY = (int) (height * regionLevel.maxTemperature);
                if (maxY < minY) maxY = minY;

                int blockX = width - maxX;
                int blockY = height - maxY;
                int blockWidth = width - minX - blockX;
                int blockHeight = height - minY - blockY;

                for (int x = 0; x < blockWidth; x++) {
                    for (int y = 0; y < blockHeight; y++) {
                        IDBiomeLevelMap.SetValue(regionLevel.idBiome, blockX + x, blockY + y);
                    }
                }
            }
        }
        
    }

    public Texture2D GetTexture2D() {
        int width = size, height = size;
        Texture2D texture = new Texture2D (width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                texture.SetPixel(x, y, _biomeDataList[IDBiomeLevelMap[x, y]].color);
            }
        }
        texture.Apply();
        return texture;
    }
    
    
    
}
