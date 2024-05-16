using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{

    private string tagtoDestroy = "Tile";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void DestroyMap()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag(tagtoDestroy);

        foreach (GameObject tile in tiles)
        {
            Destroy(tile);
        }
    }
}
