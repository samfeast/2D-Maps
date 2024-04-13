using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] MainNoiseMap(int width, int height, float scale, int startFallOff, int endFallOff, int seed) {
        Random.InitState(seed);
        int xOffset = (int) (Random.value * 65535);
        int yOffset = (int)(Random.value * 65535);

        float[,] noiseMap = new float[width, height];

        (float, float) centre = ((float)width / 2, (float)height / 2);
        float maxDist = distBetweenPoints(centre, (0.5f, 0.5f));

        if (scale <= 0) {
            scale = 0.001f;
        }

        if (startFallOff < 0) {
            startFallOff = 0;
        }

        if (endFallOff > maxDist) {
            endFallOff = (int) Mathf.Ceil(maxDist);
        }

        if (startFallOff > endFallOff) {
            startFallOff = endFallOff;
        }

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                float pointValue;

                float sampleX = x / scale + xOffset;
                float sampleY = y / scale + yOffset;

                float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);

                float distToCentre = distBetweenPoints(centre, (x + 0.5f, y + 0.5f));

                if (startFallOff < distToCentre && distToCentre < endFallOff) {
                    pointValue = perlinValue * fallOffFunction(startFallOff, endFallOff, distToCentre);
                } else if (distToCentre > endFallOff) {
                    pointValue = 0;
                } else {
                    pointValue = perlinValue;
                }

                noiseMap[x, y] = pointValue;

            }
        }

        return noiseMap;
    }

    public static float distBetweenPoints((float, float) p1, (float, float) p2) {
        float dist = Mathf.Sqrt(Mathf.Pow((p1.Item1 - p2.Item1), 2) + Mathf.Pow((p1.Item2 - p2.Item2), 2));
        return dist;
    }

    // Returns a value between 0 and 1. 1 means fulled faded (at endFallOff), 0 means not faded (at startFallOff)
    public static float fallOffFunction(float startFallOff, float endFallOff, float distToCentre) {
        return 1 - (distToCentre - startFallOff) / (endFallOff - startFallOff);
    }

    public static float[,] GenerateNoise(int width, int height, int seed) {
        Random.InitState(seed);

        float[,] noiseMap = new float[width, height];

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                noiseMap[x, y] = Random.value;
            }
        }

        return noiseMap;
    }
}
