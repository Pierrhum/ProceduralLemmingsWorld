using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomManager : MonoBehaviour
{
    [SerializeField] private SeedGenerator mushroomGenerator;
    [SerializeField] private GameObject[] mushroomRefList;
    [SerializeField] private float minMushroomSize = 5;
    [SerializeField] private float maxMushroomSize = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (Vector3 point in mushroomGenerator.GetPoints) {
            AddMushroom(point);
        }
    }

    private void AddMushroom(Vector3 position) {
        GameObject mushroom = Instantiate(mushroomRefList[Random.Range(0, mushroomRefList.Length)], transform);
        mushroom.transform.position = position;
        
        var rotation = mushroom.transform.rotation;
        rotation = Quaternion.Euler(rotation.x, Random.Range(0, 359), rotation.z);
        mushroom.transform.rotation = rotation;
        
        mushroom.transform.localScale = Vector3.one * Random.Range(minMushroomSize, maxMushroomSize);
    }
}
