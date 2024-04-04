using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int size = 256;

    public Cell[,] grid;


    // Start is called before the first frame update
    void Start()
    {
        System.Random rand = new System.Random();
        grid = new Cell[size, size];
        for (int x = 0; x < size; x++) {
            for (int y = 0; y < size; y ++) {
                Cell cell = new Cell();
                int randno = rand.Next(0, 2);
                print(randno);
                if (randno == 1) {
                    cell.isWater = true;
                } else {
                    cell.isWater = false;
                }
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
                } else {
                    Gizmos.color = Color.green;
                }
                Vector3 pos = new Vector3(x * 0.1f, y * 0.1f, 0);
                Gizmos.DrawCube(pos, 0.1f * Vector3.one);
            }
        }
    }
}
