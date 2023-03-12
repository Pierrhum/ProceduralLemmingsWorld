using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpownGrass : MonoBehaviour
{
    [SerializeField] private SeedGenerator seedGenerator;
    [SerializeField] private GameObject[] GrassRefList;
   
    private void Start() {
        if (seedGenerator == null) {
            seedGenerator = GetComponent<SeedGenerator>();
        }
        seedGenerator.GenerateSeed();
        List<Vector3> pointTreeList = seedGenerator.GetWorldPointInBiome(3);
        foreach (Vector3 point in pointTreeList) {
            AddGrass(point);
        }
    }
   
    private void AddGrass(Vector3 position) {
        int grassIndex = 0;
        GameObject grass = Instantiate(GrassRefList[grassIndex], transform);
        grass.transform.position = position;
        grass.transform.rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
    }
}
