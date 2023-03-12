using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpownLemming : MonoBehaviour
{
    [SerializeField] private SeedGenerator seedGenerator;
    [SerializeField] private GameObject[] LemmingRefList;
   
    private void Start() {
        if (seedGenerator == null) {
            seedGenerator = GetComponent<SeedGenerator>();
        }
        seedGenerator.GenerateSeed();
        List<Vector3> pointTreeList = seedGenerator.GetWorldPoints();
        foreach (Vector3 point in pointTreeList) {
            AddLemming(point);
        }
    }
   
    private void AddLemming(Vector3 position) {
        int grassIndex = 0;
        GameObject lemming = Instantiate(LemmingRefList[grassIndex], transform);
        lemming.transform.position = position;
        lemming.transform.rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
    }
}
