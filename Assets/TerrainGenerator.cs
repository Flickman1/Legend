using UnityEngine;
using System.Collections;
using Unity.Mathematics;
//adding this to test version control
public class TerrainGenerator : MonoBehaviour
{
    private int width = 100;
    private int depth = 100;
    public int height = 100;
    public float scale = 20f;
    public int seed = 0;
    private float[,] perlinNoiseCache;
    private float transitionNoiseOffset;

    void Start()
    {
        transitionNoiseOffset = UnityEngine.Random.Range(0f, 100f);
        // Generate Perlin noise cache when the game starts
        CachePerlinNoise();

        // Get the Terrain component and apply the generated terrain data
        Terrain terrain = GetComponent<Terrain>(); 
        terrain.terrainData = GenerateTerrain(terrain.terrainData); 
    }
    void Update()
    {
        
        Debug.Log("Update running");
    }
    void CachePerlinNoise()
    {
        perlinNoiseCache = new float[width, depth];
        
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float xCoord = (float)x / width * scale;
                float zCoord = (float)z / depth * scale;
                //perlinNoiseCache[x, z] = Mathf.PerlinNoise(xCoord, zCoord);
                perlinNoiseCache[x, z] = Mathf.PerlinNoise((float)x / width * 2f + transitionNoiseOffset, 0f);
            }
        }
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, height, depth);
        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }

    float SmootherStep(float edge0, float edge1, float x)
    {
        x = Mathf.Clamp01((x - edge0) / (edge1 - edge0));
        return x * x * x * (x * (x * 6 - 15) + 10);
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, depth];
        
        float transitionNoiseOffset = UnityEngine.Random.Range(0f, 100f);
        
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float xCoord, zCoord;
                float flatHeight = 0.1f;
                float hillyHeight = 0f;
                float blendFactor = 0f;

                //calculate a wavy transition boundary
                //-float noiseValue = Mathf.PerlinNoise((float)x / width * 2f + transitionNoiseOffset, 0f);
                float noiseValue = perlinNoiseCache[x, z];  // Use the cached Perlin noise value instead of recalculating it
                float transitionCenter = depth * 2f / 3f + (noiseValue - 0.5f) * 40f; // shift center +-20 blocks
                
                float transitionStart = transitionCenter - 20f;
                float transitionEnd = transitionCenter + 20f;

                // **Dynamic Waves on Transition Start/End**
                float waveFactor = Mathf.PerlinNoise(x / 10f + transitionNoiseOffset, z / 10f) * 2f; // Dynamic wave factor
                //-float waveFactor = perlinNoiseCache[x % width, z % depth] * 2f;

                transitionStart += waveFactor; // Apply waves to start
                transitionEnd += waveFactor;   // Apply waves to end            

                // Determine which quadrant we're in
                bool flatZone = z < transitionCenter;

                if (flatZone) //Q1: Grassland
                {
                    hillyHeight = flatHeight;
                }
                else          //Q2: Hills (higher elevation, rougher)
                {            
                    xCoord = (float)x / width * (scale * 0.5f);
                    zCoord = (float)z / depth * (scale * 0.5f);
                    hillyHeight = Mathf.PerlinNoise(xCoord, zCoord) * 0.3f + 0.1f;

                    // Adjust coordinates for perlin noise to match the scale and cache system
                    /*-int cacheX = Mathf.FloorToInt(xCoord / scale * width) % width;
                    int cacheZ = Mathf.FloorToInt(zCoord / scale * depth) % depth;

                    // Use the cached Perlin noise value
                    hillyHeight = perlinNoiseCache[cacheX, cacheZ] * 0.3f + 0.1f;
                    */
                }

                //smoother transition blend with more randomness
                blendFactor = SmootherStep(transitionStart, transitionEnd, z);

                // Make the transition even more dynamic with additional wave distortion
                blendFactor = Mathf.SmoothStep(0f, 1f, blendFactor);  // extra smoothing
                blendFactor = Mathf.Pow(blendFactor, 1.5f);            // slow the start a little
                blendFactor = Mathf.SmoothStep(0f, 1f, blendFactor);    // smooth again after warping

                heights[x, z] = Mathf.Lerp(flatHeight, hillyHeight, blendFactor);
            }
        }

        return heights;
    }
}
