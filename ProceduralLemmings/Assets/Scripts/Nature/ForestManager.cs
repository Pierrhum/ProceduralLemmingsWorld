using TreeEditor;
using UnityEngine;

public class ForestManager : MonoBehaviour
{
    [SerializeField] private SeedGenerator seedGenerator;
    [SerializeField] private GameObject[] treeRefList;
    [SerializeField] private Material[] matList;

    private int _cloneIndex = 0;

    private void Start() {
        foreach (GameObject treeRef in treeRefList) {
            TreeDataGenerator(treeRef.GetComponent<Tree>());
        }
        foreach (Vector3 point in seedGenerator.GetPoints) {
            AddTree(point);
        }
    }

    private void AddTree(Vector3 position) {
        GameObject tree = Instantiate(treeRefList[Random.Range(0, treeRefList.Length)], transform);
        tree.name += _cloneIndex;
        _cloneIndex++;
        tree.transform.position = position;
        tree.transform.rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
    }

    //deprecated methode. todo : fix it
    private GameObject SetTreeMeshToGameObject(Tree treePrefab) {
        GameObject tree = new GameObject("Tree");
        MeshFilter mf = tree.AddComponent<MeshFilter>();
        mf.mesh = TreeDataGenerator(treePrefab);
        MeshRenderer mr = tree.AddComponent<MeshRenderer>();
        mr.materials = matList;
        return Instantiate( tree, transform);
    }

    private Mesh TreeDataGenerator(Tree treePrefab) {
        TreeData tData = (TreeData) treePrefab.data;
        // Debug.Log("branchGroups : " + tData.branchGroups.Length);
        // Debug.Log("leafGroups : " + tData.leafGroups.Length);
        var tRoot = tData.root;
        tData.root.seed = Random.Range(0, 99999);
        foreach (TreeGroupBranch branch in tData.branchGroups) {
            branch.seed = Random.Range(0, 99999);
        }
        foreach (TreeGroupLeaf leaf in tData.leafGroups) {
            leaf.seed = Random.Range(0, 99999);
        }
        // tData.UpdateMesh(tree.transform.worldToLocalMatrix, out matList);
        return tData.mesh;
    }
}
