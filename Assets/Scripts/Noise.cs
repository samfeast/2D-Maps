using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int width, int height, float scale, int fallOffSize) {
        float[,] noiseMap = new float[width, height];

        if (scale <= 0) {
            scale = 0.001f;
        }

        fallOffSize = Mathf.Clamp(fallOffSize, 0, (int) Mathf.Ceil(Mathf.Min((float) width / 2, (float) height / 2)));

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                float sampleX = x / scale;
                float sampleY = y / scale;

                float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);

                int distToEdge = Mathf.Min(x, y, width - 1 - x, height - 1 - y);
                if (distToEdge < fallOffSize) {
                    perlinValue *= (float) distToEdge / fallOffSize;
                }

                noiseMap[x, y] = perlinValue;

            }
        }

        return noiseMap;
    }
}
