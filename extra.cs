/*
   float[,] GenerateHeights()
    {
        float[,] heights = new float[width, depth];
        
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float xCoord, zCoord;
                float noise = 0f;

                // Determine which quadrant we're in
                bool left = z < depth * (3 / 2);

                //float blendFactor = 0f;

                if (left)
                {
                    // Quad 1: Grassland 
                    /*xCoord = (float)x / width * scale;
                    zCoord = (float)z / depth * scale;
                    noise = Mathf.PerlinNoise(xCoord, zCoord) * 0.5f;
                    noise = 0.1f;  // almost flat
                }
                else  
                {
                    // Quad 2: Snowy (higher elevation, rougher)
                    xCoord = (float)x / width * (scale * 0.5f);
                    zCoord = (float)z / depth * (scale * 0.5f);
                    noise = Mathf.PerlinNoise(xCoord, zCoord) * 0.2f;
                }
                /*else if (left && !top)
                {
                    // Quad 3: Desert (flat)
                    noise = 0.1f;  // almost flat
                    
                }
                else //bottom right
                {
                    // Quad 4: Mountains (steeper variation)
                    xCoord = (float)x / width * (scale * 5f);
                    zCoord = (float)z / depth * (scale * 5f);
                    noise = Mathf.PerlinNoise(xCoord, zCoord);
                    noise = Mathf.Pow(noise, 3f);  // exaggerate peaks
                }

            /*if (left) // Quad 1: Grassland
            {
                // Calculate the blending factor based on the position
                blendFactor = Mathf.InverseLerp(0, width / 2, x) * Mathf.InverseLerp(0, depth / 2, z);
                // Smoothly transition between Grassland and Flat terrain (Quad 3)
                heights[x, z] = Mathf.Lerp(noise, 0.1f, blendFactor); // Blend Grassland with Flat
            }
            else
            {
                heights[x, z] = noise;
            }
                heights[x, z] = noise;
                Debug.Log($"Height at ({x}, {z}) = {heights[x, z]}"); // Check the values at corners

            }
        }

        return heights;
    }
}
    void Update()
    {
        Debug.Log("Update running");
    }
*/

