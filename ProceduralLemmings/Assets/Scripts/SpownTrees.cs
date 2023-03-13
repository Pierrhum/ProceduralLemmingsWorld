using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpownTrees : MonoBehaviour
{
   [SerializeField] private SeedGenerator seedGenerator;
   [SerializeField] private GameObject[] treeRefList;
   
   private void Start() {
      if (seedGenerator == null) {
         seedGenerator = GetComponent<SeedGenerator>();
      }
      seedGenerator.GenerateSeed();
      List<Vector3> pointTreeList = seedGenerator.GetWorldPointInBiome(3);
      foreach (Vector3 point in pointTreeList) {
         AddTree(point);
      }
   }
   
   private void AddTree(Vector3 position) {
      int grassIndex = 0;
      GameObject tree = Instantiate(treeRefList[grassIndex], transform);
      tree.transform.position = position;
      tree.transform.rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
   }
}
