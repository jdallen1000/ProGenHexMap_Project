using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinGridGen : MonoBehaviour
{
    Dictionary<int, GameObject> tileset;
    Dictionary<int, GameObject> tile_groups;
    public GameObject prefab_sea;
    //public GameObject prefab_sand;
    public GameObject prefab_plains;
    public GameObject prefab_forest;
    public GameObject prefab_hills;
    public GameObject prefab_mountains;


    public int map_width = 160;
    public int map_height = 90;
    public float scale = 20f;
    public int octaves = 4;
    public float lacunarity = 2f;
    public float persistence = 0.5f;

    public float easingFactor = 2f;

    List<List<int>> noise_grid = new List<List<int>>();
    List<List<GameObject>> tile_grid = new List<List<GameObject>>();

    // recommend 4 to 20
    public float magnification = 7.0f;

    public int x_offset = 0; // <- +>
    public int y_offset = 0; // v- +^

    void Start()
    {
        CreateTileset();
        CreateTileGroups();
        GenerateMap();
    }

    void CreateTileset()
    {
        /** Collect and assign ID codes to the tile prefabs, for ease of access.
            Best ordered to match land elevation. **/

        tileset = new Dictionary<int, GameObject>();
        tileset.Add(0, prefab_sea);
       // tileset.Add(1, prefab_sand);
        tileset.Add(1, prefab_plains);
        tileset.Add(2, prefab_forest);
        tileset.Add(3, prefab_hills);
        tileset.Add(4, prefab_mountains);
    }

    void CreateTileGroups()
    {
        /** Create empty gameobjects for grouping tiles of the same type, ie
            forest tiles **/

        tile_groups = new Dictionary<int, GameObject>();
        foreach (KeyValuePair<int, GameObject> prefab_pair in tileset)
        {
            GameObject tile_group = new GameObject(prefab_pair.Value.name);
            tile_group.transform.parent = gameObject.transform;
            tile_group.transform.localPosition = new Vector3(0, 0, 0);
            tile_groups.Add(prefab_pair.Key, tile_group);
        }
    }

    public void GenerateMap()
    {
        /** Generate a 2D grid using the Perlin noise function, storing it as
            both raw ID values and tile game objects **/

        for (int x = 0; x < map_width; x++)
        {
            noise_grid.Add(new List<int>());
            tile_grid.Add(new List<GameObject>());

            for (int y = 0; y < map_height; y++)
            {
                float xPos = x * 0.75f;
                float yPos = y * .85f;
                if (x % 2 == 1)
                {
                    yPos += .85f / 2;
                }
                int tile_id = GetIdUsingPerlin(xPos, yPos);
                noise_grid[x].Add(tile_id);
                CreateTile(tile_id, xPos, yPos);
                
            }
        }
    }

    int GetIdUsingPerlin(float x, float y)
    {
       /** Using a grid coordinate input, generate a Perlin noise value to be
           converted into a tile ID code. Rescale the normalized Perlin value
           to the number of tiles available. **/

       /** old perlin
        
         float raw_perlin = Mathf.PerlinNoise(
            (x - x_offset) / magnification,
            (y - y_offset) / magnification 
        );
        float eased_perlin = EasingFunction(raw_perlin, easingFactor);
        float clamp_perlin = Mathf.Clamp01(eased_perlin);
        float scaled_perlin = clamp_perlin * tileset.Count;
       
        **/

        float raw_perlin = FractalPerlinNoise(x,y);
        float eased_perlin = EasingFunction(raw_perlin, easingFactor);
        float clamp_perlin = Mathf.Clamp01(eased_perlin);
        float scaled_perlin = clamp_perlin * tileset.Count;

        // Replaced 4 with tileset.Count to make adding tiles easier
        if (scaled_perlin == tileset.Count)
        {
            scaled_perlin = (tileset.Count - 1);
        }
        return Mathf.FloorToInt(scaled_perlin);
    }

    float FractalPerlinNoise(float x, float y)
    {
        float amplitude = 1f;
        float frequency = 1f;
        float noiseValue = 0f;

        for (int o = 0; o < octaves; o++)
        {
            float xCoord = (x + x_offset) / scale * frequency;
            float yCoord = (y + y_offset) / scale * frequency;

            float perlinValue = Mathf.PerlinNoise(xCoord, yCoord) * 2 - 1; // Normalize to range [-1, 1]
            noiseValue += perlinValue * amplitude;

            frequency *= lacunarity;
            amplitude *= persistence;
        }

        return noiseValue;
    }

        void CreateTile(int tile_id, float x, float y)
    {
        /** Creates a new tile using the type ID code, group it with common
            tiles, set its position and store the gameobject. **/

        GameObject tile_prefab = tileset[tile_id];
        GameObject tile_group = tile_groups[tile_id];
        GameObject tile = Instantiate(tile_prefab, tile_group.transform);

        tile.name = string.Format("tile_x{0}_y{1}", x, y);
        tile.transform.position = new Vector3(x, y, 0); // Setting position with floats

        tile_grid[Mathf.FloorToInt(x)].Add(tile); // Using integer index for the grid
    }

    float EasingFunction(float t, float factor)
    {
        //t = t * t * t;
        //return t * factor;
        // t = Mathf.Pow(t / factor, 2);
        //t = t < 0.5 ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2;
        // t = Mathf.Sqrt(1 - Mathf.Pow(t - 1, 2));
        // t = Mathf.Clamp01(t);


        return t;//+ 0.5f;
        
        

        //t = Mathf.Clamp01(t) * (1 / easingFactor);
        //return t < 0.5 ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2; ;
    }
}