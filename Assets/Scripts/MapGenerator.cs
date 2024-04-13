using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int width;
    public int height;
    public float scale;
    public int startFallOff;
    public int endFallOff;
    public int mapSeed;
    public int textureSeed;

    public bool autoUpdate;

    public bool colourMap;

    public float waterLevel;
    public float beachLevel;

    public void GenerateMap() {
        float[,] noiseMap = Noise.MainNoiseMap(width, height, scale, startFallOff, endFallOff, mapSeed);

        MapDisplay display = FindAnyObjectByType<MapDisplay>();
        if (colourMap) {
            display.DrawColourMap(noiseMap, textureSeed, waterLevel, beachLevel);
        } else {
            display.DrawNoiseMap(noiseMap);
        }
    }
}
