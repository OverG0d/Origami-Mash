using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPerlin : MonoBehaviour
{
    //dimensions are in pixels
    public int width = 512;
    public int height = 512;

    //offset values
    public float offsetX = 0;
    public float offsetY = 0;

    //zoom value
    public float scale = 20f;

    Renderer renderer;

    private void Start()
    {
        offsetX = Random.Range(0, 99999f);
        offsetY = Random.Range(0, 99999f);
        renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        //in order to change our texture through code, we need to:
        //1. access meshrenderer
        //2. access material from meshrenderer
        //3. change material
        renderer.material.mainTexture = GenerateTexture();
    }

    Texture2D GenerateTexture() 
    {
        Texture2D texture = new Texture2D(width, height);

        //generate perlin noise map for texture
        //in order to generate, we need to loop through all of our pixels

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }
        //always apply texture after making changes to it
        //or else changes won't show
        texture.Apply();

        return texture;
    }

    Color CalculateColor(int x, int y)
    {
        /*remember to normalize values to decimal
         * whenever using perlin noise since values are
         * from 0-1 so divide by the # of pixels
         * */

        float xcoord = (float)x / width * scale + offsetX;
        float ycoord = (float)y / width * scale + offsetY;

        float sample = Mathf.PerlinNoise(xcoord, ycoord);
        return new Color(sample, sample, sample);
    }
}
