using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateEncounter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GenEncounter(TileBase currentTile)
    {
        if(currentTile.name == "sand")
        {
            Debug.Log("gen sand encounter");
        }
        if (currentTile.name == "trees")
        {
            Debug.Log("gen sand encounter");
        }
        if (currentTile.name == "grass")
        {
            Debug.Log("gen sand encounter");
        }
        if (currentTile.name == "swamp")
        {
            Debug.Log("gen sand encounter");
        }

    }
    
}
