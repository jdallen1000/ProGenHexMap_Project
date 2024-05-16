using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedGenerator : MonoBehaviour
{
    public GameObject[] weightedObjects;
    public WeightedObject weightedObjectScript;
    

    void GenerateObject()
    {
        // Calculate total weight
        float totalWeight = 0f;
        foreach (GameObject weightedObject in weightedObjects)
        {
            weightedObjectScript = weightedObject.GetComponent<WeightedObject>();
           totalWeight += weightedObjectScript.weight;
        }

        // Generate a random value between 0 and the total weight
        float randomValue = Random.Range(0f, totalWeight);

        //Find the object corresponding to the random value
        foreach (GameObject weightedObject in weightedObjects)
        {
           randomValue -= weightedObjectScript.weight;
            if (randomValue <= 0f)
            {
                 //Instantiate the selected object
                Instantiate(weightedObject.gameObject, transform.position, Quaternion.identity);
                break;
            }
        }
    }
}
