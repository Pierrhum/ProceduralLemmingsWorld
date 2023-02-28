using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/RidgedNoise")]
public class RidgedNoiseData : NoiseData
{
    [Header("Ridged Noise")]
    public int Octave = 5;
    public float Persistence = 0.5f;
    public float Lacunarity = 1.5f;

    public override float[,] GenerateMap(Vector2 Pos)
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
                float noiseValue = 0;

                for (int i = 0; i < Octave; i++) {
                    float sampleX = (x-halfWidth) / Scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y-halfHeight) / Scale * frequency + octaveOffsets[i].y;

                    float v = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
                    v = 1 - Mathf.Abs(v);
                    v *= v;
                    
                    noiseValue += v * amplitude;
                    amplitude *= Persistence;
                    frequency *= Lacunarity;
                }
                
                
                if (noiseValue > maxNoiseHeight) {
                    maxNoiseHeight = noiseValue;
                } else if (noiseValue < minNoiseHeight) {
                    minNoiseHeight = noiseValue;
                }
                noiseMap [x, y] = noiseValue;
            }
        }

        for (int y = 0; y < NoiseSize; y++) {
            for (int x = 0; x < NoiseSize; x++) {
                noiseMap [x, y] = Mathf.InverseLerp (minNoiseHeight, maxNoiseHeight, noiseMap [x, y]);
            }
        }

        return noiseMap;
    }
}
