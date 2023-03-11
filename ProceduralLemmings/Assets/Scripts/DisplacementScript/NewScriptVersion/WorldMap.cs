using UnityEngine;

public class WorldMap : MonoBehaviour
{
	public Vector2 Offset = Vector2.zero;
	
	// private BiomeRegion[] _biomeRegionList;
	private BiomeData[] _biomeDatas;
	
	
	private Renderer _textureRender;
	private MeshFilter _meshFilter;
	private MeshCollider _meshCollider;

	private void OnValidate() {
		_textureRender = GetComponent<Renderer>();
		_meshFilter = GetComponent<MeshFilter>();
		_meshCollider = GetComponent<MeshCollider>();
	}
	
	public void GenerateMesh() {
        
	}
	
	public void GenerateTexture() {
        
	}

	public void GenerateLerpEdge() {
		
	}
	
}
