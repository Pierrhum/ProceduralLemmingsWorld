using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Noise")]
public class PerlinNoiseData : NoiseData
{
    [Header("Noise")]
    public int MapSize = 200;
    public int Seed = 0;
    public float Scale = 1f;
    public int Octave = 5;
    public float Persistence = 0.5f;
    public float Lacunarity = 1.5f;
    public Vector2 Offset = Vector2.zero;
    
    public override float[,] GenerateMap()
    {
        float[,] noiseMap = new float[MapSize,MapSize];
        
        		System.Random prng = new System.Random (Seed);
        		
        		Vector2[] octaveOffsets = new Vector2[Octave];
        		for (int i = 0; i < Octave; i++) {
        			float offsetX = prng.Next (-100000, 100000) + Offset.x;
        			float offsetY = prng.Next (-100000, 100000) - Offset.y;
        			octaveOffsets [i] = new Vector2 (offsetX, offsetY);
        		}
        
        		if (Scale <= 0) {
        			Scale = 0.0001f;
        		}
        
        		float maxNoiseHeight = float.MinValue;
        		float minNoiseHeight = float.MaxValue;
        
        		float halfWidth = MapSize / 2f;
        		float halfHeight = MapSize / 2f;
        
        
        		for (int y = 0; y < MapSize; y++) {
        			for (int x = 0; x < MapSize; x++) {
        		
        				float amplitude = 1;
        				float frequency = 1;
        				float noiseHeight = 0;
        
        				for (int i = 0; i < Octave; i++) {
        					float sampleX = (x-halfWidth) / Scale * frequency + octaveOffsets[i].x;
        					float sampleY = (y-halfHeight) / Scale * frequency + octaveOffsets[i].y;
        
        					float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
        					noiseHeight += perlinValue * amplitude;
        
        					amplitude *= Persistence;
        					frequency *= Lacunarity;
        				}
        
        				if (noiseHeight > maxNoiseHeight) {
        					maxNoiseHeight = noiseHeight;
        				} else if (noiseHeight < minNoiseHeight) {
        					minNoiseHeight = noiseHeight;
        				}
        				noiseMap [x, y] = noiseHeight;
        			}
        		}
        
        		for (int y = 0; y < MapSize; y++) {
        			for (int x = 0; x < MapSize; x++) {
        				noiseMap [x, y] = Mathf.InverseLerp (minNoiseHeight, maxNoiseHeight, noiseMap [x, y]);
        			}
        		}
        
        		return noiseMap;
    }

	void OnValidate() {
		if (MapSize < 1) {
			MapSize = 1;
		}
		if (Octave < 0) {
			Octave = 0;
		}
	}
}
