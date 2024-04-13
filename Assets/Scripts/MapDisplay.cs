using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer textureRender;

    public void DrawNoiseMap(float[,] noiseMap) {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        Color[] colourMap = new Color[width * height];
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }

        texture.SetPixels(colourMap);
        texture.Apply();

        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3(width, 1, height);
    }

    public void DrawColourMap(float[,] noiseMap, float waterLevel, float beachLevel) {

        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        Color[] colourMap = new Color[width * height];

        Color[] beachColours = new Color[4];
        beachColours[0] = new Color((float)246 / 255, (float)215 / 255, (float)176 / 255, 1);
        beachColours[1] = new Color((float)242 / 255, (float)210 / 255, (float)169 / 255, 1);
        beachColours[2] = new Color((float)236 / 255, (float)204 / 255, (float)162 / 255, 1);
        beachColours[3] = new Color((float)231 / 255, (float)196 / 255, (float)150 / 255, 1);

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {

                if (noiseMap[x, y] < waterLevel) {
                    float darkness = Mathf.Max(noiseMap[x, y] / waterLevel + 0.3f, 0.3f);
                    colourMap[y * width + x] = new Color(0f, 0f, darkness, 1);
                } else if (noiseMap[x, y] < beachLevel) {
                    int rand = Mathf.FloorToInt(Random.value * (3.9999f));
                    colourMap[y * width + x] = beachColours[rand];
                } else {
                    colourMap[y * width + x] = Color.green;
                }
                
            }
        }

        texture.SetPixels(colourMap);
        texture.Apply();

        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3(width, 1, height);
    }
}
