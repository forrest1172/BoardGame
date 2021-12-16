﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateEncounter : MonoBehaviour
{
    public EncounterCards[] sand;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GenEncounter(TileBase currentTile)
    {
        if(currentTile.name == "sand")
        {
            int i = Random.Range(0, sand.Length);

            Instantiate(sand[i]);

            Debug.Log("gen sand encounter");
            
            
        }
        if (currentTile.name == "trees")
        {
            Debug.Log("gen tree encounter");
        }
        if (currentTile.name == "grass")
        {
            Debug.Log("gen grass encounter");
        }
        if (currentTile.name == "swamp")
        {
            Debug.Log("gen swamp encounter");
        }

    }
    
}
