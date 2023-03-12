using UnityEngine;

public class GroundManager : MonoBehaviour
{
    
    [SerializeField] private Material groundMat;

    private MeshRenderer _groundRenderer;
    
    void Start() {
        _groundRenderer = transform.GetComponent<MeshRenderer>();
        _groundRenderer.material = groundMat;
    }
    
}
