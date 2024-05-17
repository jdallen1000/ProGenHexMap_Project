using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POI_Generator : MonoBehaviour
{
    public string tileTag = "Tile";
    public TileTypes POI_GEN_tileTarget = TileTypes.Plains; 
    public GameObject prefab_townHex;
    public int chance = 1;

    private List<GameObject> tilesList = new List<GameObject>();

    void FindTiles() // Finds all the tiles in the game and adds them to a tilelist
    {
        tilesList.Clear();
        GameObject[] tiles = GameObject.FindGameObjectsWithTag(tileTag);
        foreach (GameObject tile in tiles)
        {
            tilesList.Add(tile);
        }
    }

    public void GeneratePOI() // Generates POIs on the Grid map on the tile types picked
    {
        FindTiles();
        for (int x = 0; x < tilesList.Count; x++)
        {
            Tile tileComponent = tilesList[x].GetComponent<Tile>();
            if (tileComponent != null && tileComponent.tileType == POI_GEN_tileTarget)
            {
                int tilePOIChance = Random.Range(1, chance + 1);
                if (tilePOIChance == chance)
                {
                    Instantiate(prefab_townHex, tilesList[x].transform.position, Quaternion.identity, tilesList[x].transform.parent);
                    Destroy(tilesList[x]);
                }
            }
        }
    }
}
