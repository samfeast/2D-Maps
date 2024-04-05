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
        // Generate random coordinate to act as (0,0) in the noise map.
        System.Random rng = new System.Random();
        int xOrigin = rng.Next(-65536, 65536);
        int yOrigin = rng.Next(-65536, 65536);

        // The noise map is two cells bigger in each dimension that the map being displayed, in order to find the state of cells just outside the border.
        noiseMap = new float[size+2, size+2];
        for (int x = 0; x < size+2; x++) {
            for (int y = 0; y < size+2; y++) {
                // The denominator scales the value to ensure it is between 0 and 1, since PerlinNoise() returns the same value for integer inputs.
                float noiseValue = Mathf.PerlinNoise((float) xOrigin + x / ((size+2) * 0.125f), (float) yOrigin + y / ((size+2) * 0.125f));
                // PerlinNoise() can return values slightly below 0 or above 1, therefore clamp these values.
                if (noiseValue > 1) {
                    noiseMap[x, y] = 1;
                } else if (noiseValue < 0) {
                    noiseMap[x, y] = 0;
                } else {
                    noiseMap[x, y] = noiseValue;
                }
            }
        }

        grid = new Cell[size, size];
        for (int x = 0; x < size; x++) {
            for (int y = 0; y < size; y++) {
                // The cells on the map start at (1,1) in the noise map to account for a 1 cell border.
                Cell cell = new Cell();
                // The cell is water if its below the waterlevel.
                cell.isWater = noiseMap[x+1, y+1] < waterLevel;
                grid[x, y] = cell;
            }
        }
    }

    // Draw the map using gizmos.
    void OnDrawGizmos() {
        // Don't try and draw anything if the program isn't running.
        if (!Application.isPlaying) {
            return;
        }

        for (int x = 0; x < size; x++) {
            for (int y = 0; y < size; y++) {
                // Draw the map. Water is blue, beaches are yellow, and land is green.
                Cell cell = grid[x, y];
                if (cell.isWater) {
                    Gizmos.color = Color.blue;
                } else if (AdjacentWater(x, y)) {
                    Gizmos.color = Color.yellow;
                } else {
                    Gizmos.color = Color.green;
                }
                // The position and scale of the cubes are divided by 10 to prevent the map being really big.
                // This is needed because unity loses precision beyond ~10000 units.
                Vector3 pos = new Vector3(x * 0.1f, y * 0.1f, 0);
                Gizmos.DrawCube(pos, Vector3.one * 0.1f);
            }
        }

        // Draw the top and bottom sides of the border around the map.
        // Land is pink and water is a shade of gray (white = 0).
        for (int x = 0; x < size + 2; x++) {
            float noiseValue = noiseMap[x, 0];
            if (noiseValue > waterLevel) {
                Gizmos.color = new Color(1, 0, 1, 1);
            }
            else {
                Gizmos.color = new Color(1 - noiseValue, 1 - noiseValue, 1 - noiseValue, 1);
            }
            Vector3 pos = new Vector3(x * 0.1f - 0.1f, -0.1f, 0);
            Gizmos.DrawCube(pos, 0.1f * Vector3.one);
            noiseValue = noiseMap[x, size + 1];
            if (noiseValue > waterLevel) {
                Gizmos.color = new Color(1, 0, 1, 1);
            }
            else {
                Gizmos.color = new Color(1 - noiseValue, 1 - noiseValue, 1 - noiseValue, 1);
            }
            pos = new Vector3(x * 0.1f - 0.1f, size * 0.1f, 0);
            Gizmos.DrawCube(pos, 0.1f * Vector3.one);
        }

        // Draw the left and right sides of the border around the map.
        // Land is pink and water is a shade of gray (white = 0).
        // This starts at index 1 and doesn't loop over the last element in the noise map, since these are the corners and have already been drawn above.
        for (int y = 1; y < size + 1; y++) {
            float noiseValue = noiseMap[0, y];
            if (noiseValue > waterLevel) {
                Gizmos.color = new Color(1, 0, 1, 1);
            }
            else {
                Gizmos.color = new Color(1 - noiseValue, 1 - noiseValue, 1 - noiseValue, 1);
            }
            Vector3 pos = new Vector3(-0.1f, y * 0.1f - 0.1f, 0);
            Gizmos.DrawCube(pos, 0.1f * Vector3.one);
            noiseValue = noiseMap[size + 1, y];
            if (noiseValue > waterLevel) {
                Gizmos.color = new Color(1, 0, 1, 1);
            }
            else {
                Gizmos.color = new Color(1 - noiseValue, 1 - noiseValue, 1 - noiseValue, 1);
            }
            pos = new Vector3(size * 0.1f, y * 0.1f - 0.1f, 0);
            Gizmos.DrawCube(pos, 0.1f * Vector3.one);
        }

        // Draw the noise map next to the actual map for debugging purposes.
        // Black is land, green is land but within 0.05 of the water level, red is water but within 0.05 of the water level, and water is gray (white = 0).
        for (int x = 0; x < size + 2; x++) {
            for (int y = 0; y < size + 2; y++) {
                float noiseValue = noiseMap[x, y];
                if (noiseValue > waterLevel + 0.05f) {
                    Gizmos.color = Color.black;
                }
                else if (noiseValue > waterLevel) {
                    Gizmos.color = Color.green;
                }
                else if (noiseValue > waterLevel - 0.05f) {
                    Gizmos.color = Color.red;
                }
                else {
                    Gizmos.color = new Color(1 - noiseValue, 1 - noiseValue, 1 - noiseValue, 1);
                }
                Vector3 pos = new Vector3(x * 0.1f + (size + 2) * 0.1f, y * 0.1f - 0.1f, 0);
                Gizmos.DrawCube(pos, 0.1f * Vector3.one);
            }
        }
    }

    // If any of the adjacent cells (NESW) are water, return true, otherwise return false.
    bool AdjacentWater(int x, int y) {
        // Since the noise map is slightly bigger than the map itself, 1 is added to all coordinates.
        if (noiseMap[x+1, y+2] < waterLevel) {
            return true;
        } else if (noiseMap[x+2, y+1] < waterLevel) {
            return true;
        }
        else if (noiseMap[x+1, y] < waterLevel) {
            return true;
        }
        else if (noiseMap[x, y+1] < waterLevel) {
            return true;
        } else {
            return false;
        }
    }
}
