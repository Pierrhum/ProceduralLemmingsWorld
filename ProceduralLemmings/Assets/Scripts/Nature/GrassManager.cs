using UnityEngine;

public class GrassManager : MonoBehaviour
{
	[SerializeField] private SeedGenerator grassGenerator;
	[SerializeField] private GameObject[] grassRefList;
	[SerializeField] [Range(0.0f, 1.0f)] private float fernLuck = 1.0f;
	
	private void Start() {
		foreach (Vector3 point in grassGenerator.GetPoints) {
			AddGrass(point);
		}
	}

	private void AddGrass(Vector3 position) {
		float randomLuck = Random.Range(0.0f, 1.0f);
		int grassIndex = 0;
		if (randomLuck < fernLuck) grassIndex = 1;
		GameObject grass = Instantiate(grassRefList[grassIndex], transform);
		grass.transform.position = position;
		grass.transform.rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
	}
}
