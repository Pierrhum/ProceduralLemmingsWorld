using UnityEngine;

public class BushManager : MonoBehaviour
{
    [SerializeField] private SeedGenerator grassGenerator;
    [SerializeField] private GameObject[] grassRefList;
	
    private void Start() {
        foreach (Vector3 point in grassGenerator.GetPoints) {
            AddGrass(point);
        }
    }

    private void AddGrass(Vector3 position) {
        GameObject grass = Instantiate(grassRefList[Random.Range(0, grassRefList.Length)], transform);
        grass.transform.position = position;
        grass.transform.rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
    }
}
