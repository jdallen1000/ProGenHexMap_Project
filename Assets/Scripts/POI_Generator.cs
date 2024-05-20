using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class POI_Generator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase targetTile;  // The tile type you want to compare against
    public TileBase POI_Tile;
    public int chance = 1;

    public void GeneratePOI()
    {
        BoundsInt bounds = tilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                TileBase currentTile = tilemap.GetTile(tilePosition);

                // Compare the current tile to the targetTile instance
                if (currentTile == targetTile)
                {
                    int tilePOIChance = Random.Range(1, chance + 1);
                    if (tilePOIChance == chance)
                    {
                        tilemap.SetTile(tilePosition, POI_Tile);
                    }
                }
            }
        }
    }
}
