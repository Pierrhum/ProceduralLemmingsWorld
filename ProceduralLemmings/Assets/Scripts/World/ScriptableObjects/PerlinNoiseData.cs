using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/PerlinNoise")]
public class PerlinNoiseData : NoiseData
{
	[Header("Perlin Noise")]
    public int Octave = 5;
    //[Range(0.0f, 1.0f)] 
    public float Persistence = 0.5f;
    //[Range(0.0f, 1.0f)]
    public float Lacunarity = 1f;
    
    public override float[,] GenerateMap(Vector2 Pos)
    {
        float[,] noiseMap = new float[NoiseSize,NoiseSize];
        System.Random prng = new System.Random (Seed);
        
        float offsetX = prng.Next (-100000, 100000);
        float offsetY = prng.Next (-100000, 100000);

        if (Scale <= 0) {
        	Scale = 0.0001f;
        }

        float halfSize = NoiseSize / 2f;

        // maxNoiseHeight = float.MinValue;
        // minNoiseHeight = float.MaxValue;

        for (int y = 0; y < NoiseSize; y++) {
        	for (int x = 0; x < NoiseSize; x++) {
        
        		float amplitude = 1;
        		float frequency = 1;
        		float noiseHeight = 0;

        		for (int i = 0; i < Octave; i++) {
        			float sampleX = (Pos.x + (x-halfSize)) / Scale * frequency + offsetX;
        			float sampleY = (Pos.y + (y-halfSize)) / Scale * frequency + offsetY;

        			float PerlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
        			noiseHeight += PerlinValue * amplitude;

        			amplitude *= Persistence;
        			frequency *= Lacunarity;
        		}

                // Min and Max Update
        		if (noiseHeight > maxNoiseHeight) maxNoiseHeight = noiseHeight;
        		else if (noiseHeight < minNoiseHeight) minNoiseHeight = noiseHeight;
        		
                // NoiseMap sample
        		noiseMap [x, y] = noiseHeight;
        	}
        }

        noiseMap = NormalizeNoise(noiseMap);

        return noiseMap;
    }

	void OnValidate() {
		if (Octave < 0) {
			Octave = 0;
		}
	}
}
