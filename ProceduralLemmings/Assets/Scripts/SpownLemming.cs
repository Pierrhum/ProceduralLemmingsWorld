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
        List<Vector3> spawnSpoints = pointTreeList;

        if(spawnSpoints.Count > 0)
            for (int i = 0; i < number; i++)
            {
                int spawn = Random.Range(0, spawnSpoints.Count);
                List<Vector3> path = new List<Vector3>();
                // Generate a path for half of lemmings
                if (i % 2 == 0)
                {
                    for (int p = 0; p < 5; p++)
                    {
                        int tries=0;
                        int random = Random.Range(0, pointTreeList.Count);
                        while (Vector3.Distance(spawnSpoints[spawn], pointTreeList[random]) > 10f)
                        {
                            random = Random.Range(0, pointTreeList.Count);
                            tries++;
                            if (tries > 200) break;
                        }
                        if(tries < 200)
                            path.Add(pointTreeList[random]);
                    }
                }
                AddLemming(spawnSpoints[spawn], biome, path);
                spawnSpoints.RemoveAt(spawn);
                if (spawnSpoints.Count <= 0) break;
            }
    }
   
    private void AddLemming(Vector3 position, int biome, List<Vector3> path) {
        GameObject lemming = Instantiate(Lemming, transform);
        if (biome == (int)Biome.Ocean)
            position -= new Vector3(0,lemming.transform.localScale.y / 2,0);
        lemming.transform.position = position;
        lemming.transform.rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
        lemming.GetComponent<Lemming>().SetBiome(biome, path);
    }
}
