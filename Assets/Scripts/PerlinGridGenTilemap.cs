using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PerlinGridGenTilemap : MonoBehaviour
{
    public Tilemap tilemap;
    Dictionary<int, TileBase> tileset;

    public TileBase tile_sea;
    //public TileBase tile_sand;
    public TileBase tile_plains;
    public TileBase tile_forest;
    public TileBase tile_hills;
    public TileBase tile_mountains;
    
    public int map_width = 160;
    public int map_height = 90;
    public float scale = 20f;
    public int octaves = 4;
    public float lacunarity = 2f;
    public float persistence = 0.5f;
    public float easingFactor = 2f;
    public float magnification = 7.0f;
    public int x_offset = 0;
    public int y_offset = 0;

    List<List<int>> noise_grid = new List<List<int>>();

    void Start()
    {
        CreateTileset();
        GenerateMap();
    }

    void CreateTileset()
    {
        /** Collect and assign ID codes to the tile prefabs, for ease of access.
            Best ordered to match land elevation. **/
        tileset = new Dictionary<int, TileBase>();
        tileset.Add(0, tile_sea);
        //tileset.Add(1, tile_sand);
        tileset.Add(1, tile_plains);
        tileset.Add(2, tile_forest);
        tileset.Add(3, tile_hills);
        tileset.Add(4, tile_mountains);
    }

    public void GenerateMap()
    {
        /** Generate a 2D grid using the Perlin noise function, storing it as
            both raw ID values and tile game objects **/

        for (int x = 0; x < map_width; x++)
        {
            noise_grid.Add(new List<int>());

            for (int y = 0; y < map_height; y++)
            {

                int tile_id = GetIdUsingPerlin(x, y);
                noise_grid[x].Add(tile_id);
                SetTile(tile_id, y, x);
            }
        }
    }

    int GetIdUsingPerlin(float x, float y)
    {
        /** Using a grid coordinate input, generate a Perlin noise value to be
            converted into a tile ID code. Rescale the normalized Perlin value
            to the number of tiles available. **/

        float raw_perlin = FractalPerlinNoise(x, y);
        float eased_perlin = EasingFunction(raw_perlin, easingFactor);
        float clamp_perlin = Mathf.Clamp01(eased_perlin);
        float scaled_perlin = clamp_perlin * tileset.Count;

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

    void SetTile(int tile_id, int x, int y)
    {
        /** Sets the tile in the tilemap using the type ID code **/
        TileBase tile = tileset[tile_id];
        tilemap.SetTile(new Vector3Int(x, y, 0), tile);
    }

    float EasingFunction(float t, float factor)
    {
        return t;
    }

    public void ClearMap()
    {
        /** Clears all the tiles in the tilemap **/
        for (int x = 0; x < map_width; x++)
        {
            for (int y = 0; y < map_height; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), null);
            }
        }
        noise_grid.Clear();
    }
}


