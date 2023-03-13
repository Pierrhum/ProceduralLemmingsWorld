using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Biome {Ocean = 0, Grass, Desert, Forest, Snow}

public class SpownLemming : MonoBehaviour
{
    private SeedGenerator seedGenerator;
    [SerializeField] private GameObject Lemming;
   
    [SerializeField] private int OceanLemmings;
    [SerializeField] private int GrassLemmings;
    [SerializeField] private int DesertLemmings;
    [SerializeField] private int ForestLemmings;
    [SerializeField] private int SnowLemmings;
    private void Start() {
        seedGenerator = GetComponent<SeedGenerator>();
        
        SpawnAtBiome(Biome.Ocean, OceanLemmings);
        SpawnAtBiome(Biome.Grass, GrassLemmings);
        SpawnAtBiome(Biome.Desert, DesertLemmings);
        SpawnAtBiome(Biome.Forest, ForestLemmings);
        SpawnAtBiome(Biome.Snow, SnowLemmings);
    }

    private void SpawnAtBiome(Biome biomeName, int number)
    {
        int biome = (int)biomeName;
        
        seedGenerator.GenerateSeed();
        List<Vector3> pointTreeList = seedGenerator.GetWorldPointInBiome(biome);

        if(pointTreeList.Count > 0)
            for (int i = 0; i < number; i++)
            {
                int random = Random.Range(0, pointTreeList.Count);
                AddLemming(pointTreeList[random], biome);
            }
    }
   
    private void AddLemming(Vector3 position, int biome) {
        GameObject lemming = Instantiate(Lemming, transform);
        if (biome == (int)Biome.Ocean)
            position -= new Vector3(0,lemming.transform.localScale.y / 2,0);
        lemming.transform.position = position;
        lemming.transform.rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
        lemming.GetComponent<Lemming>().SetBiome(biome);
    }
}
