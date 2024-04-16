using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Biomes
{
    public static (int, int)[,] BiomeMap(int width, int height, int xDivisions, int yDivisions) {
        if (xDivisions <= 0) xDivisions = 1;
        if (yDivisions <= 0) yDivisions = 1;

        if (xDivisions >= width) xDivisions = width;
        if (yDivisions >= height) yDivisions = height;

        (int, int)[,] chunkCorners = new (int, int)[xDivisions, yDivisions];

        int basePartitionWidth = width / xDivisions;
        int basePartitionHeight = height / yDivisions;

        for (int y = 0; y < chunkCorners.GetLength(1); y++) {
            for (int x = 0; x < chunkCorners.GetLength(0); x++) {
                if (x == 0) {
                    chunkCorners[x, y].Item1 = basePartitionWidth;
                } else {
                    chunkCorners[x, y].Item1 = basePartitionWidth + chunkCorners[x - 1, y].Item1;
                }

                if (y == 0) {
                    chunkCorners[x, y].Item2 = basePartitionHeight;
                }
                else {
                    chunkCorners[x, y].Item2 = basePartitionHeight + chunkCorners[x, y - 1].Item2;
                }

                if (x < width % xDivisions) chunkCorners[x, y].Item1++;
                if (y < height % yDivisions) chunkCorners[x, y].Item2++;
            }
        }

        (int, int)[,] samplePoints = new (int, int)[xDivisions, yDivisions];

        for (int y = 0; y < chunkCorners.GetLength(1); y++) {
            for (int x = 0; x < chunkCorners.GetLength(0); x++) {

                if (x == 0) {
                    samplePoints[x, y].Item1 = Random.Range(1, chunkCorners[x, y].Item1);
                }
                else {
                    samplePoints[x, y].Item1 = Random.Range(chunkCorners[x - 1, y].Item1 + 1, chunkCorners[x, y].Item1);
                }

                if (y == 0) {
                    samplePoints[x, y].Item2 = Random.Range(1, chunkCorners[x, y].Item2);
                }
                else {
                    samplePoints[x, y].Item2 = Random.Range(chunkCorners[x, y - 1].Item2 + 1, chunkCorners[x, y].Item2);
                }

                Debug.Log(samplePoints[x, y].ToString());
            }
        }

        // Sample points are randomly selected points distributed in such a way that there is one point in each chunk

        // Randomly assign each sample point a biome.

        // Then go through all the points on the map (width * height) and find the closest point.
        // The closest sample point to a cell could be in the current chunk, any of the 8 surrounding chunks, or 2 chunks in any of the cardinal directions.
        // Therefore, check the distance to the the sample points in these 13 chunks.
        // The biome for the cell is determined as follows, where 'blendThreshold' is a value passed in, and 'closestSample' is the distance to the nearest sample point.
        // If the distance to the other 12 sample points is larger than 'closestSample + blendThreshold', then the cell is the biome assigned to the sample point.
        // If there are other sample points that are a distance shorter than 'closestSample + blendThreshold', the biome is randomly determined by weighting the distances.
        // I.e, if the the closest sample point is 10 away, and the second closest is also 10 away, there should be a 50% chance that the cell is in each biome.
        // Likewise, if the closest sample is 10 away, and the second closest is 20 away, there should be a 66% chance the biome is from the sample 10 away, and 33% it is from the one 20 away.
        // This second example assumes the blend threshold is at least 10, and that only 1 other point lies inside the blend threshold.

        return samplePoints;
    }
}
