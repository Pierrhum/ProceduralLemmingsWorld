using UnityEngine;

public abstract class NoiseData : ScriptableObject
{
    [Header("Noise")]
    public int Seed = 0;
    public float Scale = 1f;
    public Vector2 Offset = Vector2.zero;
    
    public float[,] NoiseMap { get; }
    private float[,] _noiseMap;

    private void OnEnable() {
        _noiseMap = GenerateMap();
    }

    // 256 * 256
    public static int NoiseSize = 256;
    // Type
    public abstract float[,] GenerateMap();
}
