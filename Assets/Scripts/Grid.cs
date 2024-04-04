using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int size;
    public float waterLevel;

    public Cell[,] grid;
    public float[,] noiseMap;


    // Start is called before the first frame update
    void Start()
    {

        System.Random rng = new System.Random();
        int xOrigin = rng.Next(-65536, 65536);
        int yOrigin = rng.Next(-65536, 65536);

        noiseMap = new float[size, size];
        for (int x = 0; x < size; x++) {
            for (int y = 0; y < size; y++) {
                float noiseValue = Mathf.PerlinNoise((float) (8 * (x + xOrigin)) / size, (float) (8 * (y + yOrigin)) / size);
                noiseMap[x, y] = noiseValue;
            }
        }

        grid = new Cell[size, size];
        for (int x = 0; x < size; x++) {
            for (int y = 0; y < size; y++) {
                Cell cell = new Cell();
                cell.isWater = noiseMap[x, y] < waterLevel;
                grid[x, y] = cell;
            }
        }
    }

    void OnDrawGizmos() {
        if (!Application.isPlaying) {
            return;
        }

        for (int x = 0; x < size; x++) {
            for (int y = 0; y < size; y++) {
                Cell cell = grid[x, y];
                if (cell.isWater) {
                    Gizmos.color = Color.blue;
                } else if (AdjacentWater(x, y)) {
                    Gizmos.color = Color.yellow;
                } else {
                    Gizmos.color = Color.green;
                }
                Vector3 pos = new Vector3(x * 0.1f, y * 0.1f, 0);
                Gizmos.DrawCube(pos, 0.1f * Vector3.one);
            }
        }
    }

    bool AdjacentWater(int x, int y) {
        if (y == size - 1 || noiseMap[x, y+1] < waterLevel) {
            return true;
        } else if (x == size - 1 || noiseMap[x+1, y] < waterLevel) {
            return true;
        }
        else if (y == 0 || noiseMap[x, y-1] < waterLevel) {
            return true;
        }
        else if (x == 0 || noiseMap[x-1, y] < waterLevel) {
            return true;
        } else {
            return false;
        }
    }
}
