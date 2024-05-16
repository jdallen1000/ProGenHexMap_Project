using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public int numRows = 5; // Number of rows in the grid
    public int numColumns = 5; // Number of columns in the grid
    public float hexRadius = 1f; // Radius of each hexagon
    public Vector2 gridOrigin = Vector2.zero;
    public GameObject[] gridTiles;
    public WeightedObject weightedObjectScript;

    void Start()
    {
       // GenerateHexGrid();
    }

    void GenerateHexGrid()
    {
        float hexWidth = hexRadius * 2f;
        float xOffset = hexWidth * 0.75f;
        float yOffset = 0.43f;
            

        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                float xPos = col * xOffset;
                if (row % 2 == 1) // Shift every other row
                    xPos += xOffset * 0.5f;

                float yPos = row * yOffset;

                // Instantiate hex tile
                Vector2 hexPosition = new Vector2(xPos + gridOrigin.x, yPos + gridOrigin.y);
                PickAndSpawn(hexPosition, Quaternion.identity);
                //GenerateObject(hexPosition, Quaternion.identity);
            }
        }
    }
    void PickAndSpawn(Vector2 hexPosition, Quaternion spawnRotation)
    {
        int randomIndex =Random.Range(0, gridTiles.Length);
        GameObject hexGO = Instantiate(gridTiles[randomIndex], hexPosition, Quaternion.identity, transform);
    }

    void GenerateObject(Vector2 hexPosition, Quaternion spawnRotation)
    {
        // Calculate total weight
        float totalWeight = 0f;
        foreach (GameObject weightedObject in gridTiles)
        {
            weightedObjectScript = weightedObject.GetComponent<WeightedObject>();
            totalWeight += weightedObjectScript.weight;
        }

        // Generate a random value between 0 and the total weight
        float randomValue = Random.Range(0f, totalWeight);

        //Find the object corresponding to the random value
        foreach (GameObject weightedObject in gridTiles)
        {
            randomValue -= weightedObjectScript.weight;
            if (randomValue <= 0f)
            {
                //Instantiate the selected object
                GameObject hexGO = Instantiate(weightedObject.gameObject, hexPosition, Quaternion.identity, transform);
                break;
            }
        }
    }
}
    