using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/PerlinNoise")]
public class PerlinNoiseData : NoiseData
{
	[Header("Perlin Noise")]
    public int Octave = 5;
    public float Persistence = 0.5f;
    public float Lacunarity = 1.5f;
    
    public override float[,] GenerateMap()
    {
        float[,] noiseMap = new float[NoiseSize,NoiseSize];
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

        float halfWidth = NoiseSize / 2f;
        float halfHeight = NoiseSize / 2f;


        for (int y = 0; y < NoiseSize; y++) {
        	for (int x = 0; x < NoiseSize; x++) {
        
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

        for (int y = 0; y < NoiseSize; y++) {
        	for (int x = 0; x < NoiseSize; x++) {
        		noiseMap [x, y] = Mathf.InverseLerp (minNoiseHeight, maxNoiseHeight, noiseMap [x, y]);
        	}
        }

        return noiseMap;
    }

	void OnValidate() {
		if (Octave < 0) {
			Octave = 0;
		}
	}
}
