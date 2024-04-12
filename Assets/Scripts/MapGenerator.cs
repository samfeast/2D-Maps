using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int width;
    public int height;
    public float scale;
    public int fallOffSize;

    public bool autoUpdate;

    public bool colourMap;

    public float waterLevel;

    public void GenerateMap() {
        float[,] noiseMap = Noise.GenerateNoiseMap(width, height, scale, fallOffSize);

        MapDisplay display = FindAnyObjectByType<MapDisplay>();
        if (colourMap) {
            display.DrawColourMap(noiseMap, waterLevel);
        } else {
            display.DrawNoiseMap(noiseMap);
        }
    }
}
