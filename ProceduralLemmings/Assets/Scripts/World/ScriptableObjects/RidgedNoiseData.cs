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
        
        float offsetX = prng.Next (-100000, 100000);
        float offsetY = prng.Next (-100000, 100000);

        if (Scale <= 0) {
            Scale = 0.0001f;
        }

        //float maxNoiseHeight = float.MinValue;
        //float minNoiseHeight = float.MaxValue;
        
        float halfSize = NoiseSize / 2f;


        for (int y = 0; y < NoiseSize; y++) {
            for (int x = 0; x < NoiseSize; x++) {
        
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < Octave; i++) {
                    float sampleX = (Pos.x + (x-halfSize)) / Scale * frequency + offsetX;
                    float sampleY = (Pos.y + (y-halfSize)) / Scale * frequency + offsetY;

                    float v = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
                    v = 1 - Mathf.Abs(v);
                    v *= v;
                    
                    noiseHeight += v * amplitude;
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

        for (int y = 0; y < NoiseSize; y++) {
            for (int x = 0; x < NoiseSize; x++) {
                noiseMap [x, y] = Mathf.InverseLerp (minNoiseHeight, maxNoiseHeight, noiseMap [x, y]);
            }
        }

        return noiseMap;
    }
}
