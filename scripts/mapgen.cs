using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using Mirror;
using System.Linq;

public class mapgen : NetworkBehaviour
{
    public double width = 10;
    public double height = 10;
    public double[][] value;
    public float scale = 5f;
    public double exponent;
    public float fudge_factor;

    public float offsetX = 100;
    public float offsetY = 100;

    public GameObject spawnPoint;
    public List<GameObject> spawns = new List<GameObject>();
    public int maxCityLimit = 10;
   
    public GameObject player;

   
    
    

    public GameObject cityGo;
    public Tilemap tileMap;
    public TileBase[] water;
    public TileBase[] desert;
    public TileBase[] grass;
    public TileBase[] swamp;
    public TileBase[] beach;
    public TileBase[] forest;
    public TileBase[] iceMountains;
    public TileBase[] Cities;
    public TileBase[] encounterSpot;
    public TileBase[] debugSprite;
    public TextMeshProUGUI cityCount;
    private int numOfCities = 0;
    private bool isCityMax = false;
    public MapSeed mapSeed;
    private double e;
    public bool citiesSpawned = false;
    public int i = 0;

    public TextMeshProUGUI seedText;

    public int currentSeed;
   
    //public string MyGameSeed;
    
   
    public void GetSeed()
    {
        i = 0;
        GenCities();
        citiesSpawned = true;

        currentSeed = mapSeed.GameSeed;
        //currentSeed = MyGameSeed.GetHashCode();

        UnityEngine.Random.InitState(currentSeed);
        Debug.Log("got seed");
        RandomizeMap();
        
        
        
    }



    public void GenCities()
    {
        if (citiesSpawned == false)
        {
            for (int c = 0; c < maxCityLimit; c++)
            {
               GameObject spawn = Instantiate(spawnPoint, new Vector3(0, 0, 0), Quaternion.identity);
                spawns.Add(spawn);
                //NetworkServer.Spawn(spawnPoint);

            }

        }
        else return;
    }

    public void SpawnPlayer()
    {
        foreach (var sp in spawns)
        {
            sp.GetComponent<spawnPointData>().GetNeighbors();
        }
        playerBehaviour[] players = FindObjectsOfType<playerBehaviour>();
        foreach(var p in players)
        {
            
            int b = UnityEngine.Random.Range(0, spawns.Count);
            p.gameObject.transform.position = spawns[b].transform.position;
            
            string sp = spawns[b].gameObject.GetComponent<spawnPointData>().nameString;
            p.GetHome(sp);
        }
        
    }
    

    public void RandomizeMap()

    {
        tileMap.ClearAllTiles();
        Mathf.Clamp(i, 0, maxCityLimit);
        
        seedText.text = currentSeed.ToString();

        offsetX = UnityEngine.Random.Range(0, 9999);
        offsetY = UnityEngine.Random.Range(0, 9999);

        isCityMax = false;
        numOfCities = 0;






          

        for (int x = 0; x < width; x++)
        {

            for (int y = 0; y < height; y++)
            {
                //cityCount.text = numOfCities.ToString(); 
                
               if(x == width - 1 && y == height - 1)
                {
                    Debug.Log("map done...");
                    
                    
                }

                if (numOfCities >= maxCityLimit)
                {
                    isCityMax = true;

                }

                /*else if(x == width && y == height)
                {
                 
                    SpawnPlayer(con);
                }*/

                double nx = x / width - 0.5f;
                double ny = y / height - 0.5f;
                
                
                double m = CalculateMoisture(x, y);
                double d = 2 * Math.Max(Math.Abs(nx), Math.Abs(ny));
                e = 1 * CalculateHeight(1 * x, 1 * y) 
                + 0.5f * CalculateHeight(2 * x, 2 * y) 
                + 0.25f * CalculateHeight(4 * x, 4 * y);
                //raise d to a power. the higher the power the larger the islands
                d = Math.Pow(d, 0.8);
                e = (1 + e - d) / 2;
                double E = Math.Pow(e * fudge_factor , exponent);
                
                
                
                Biome(E,x,y,m);
                






            }
        }
        
         void Biome(double E, int x, int y, double m)
        {
            if (E < 0.3) tileMap.SetTile(new Vector3Int(x, y, 0), water[0]);
            else if (E < 0.35)
            {
                int isCity = UnityEngine.Random.Range(0, 101);
                if ((isCity == 45) && (isCityMax == false))
                {
                    tileMap.SetTile(new Vector3Int(x, y, 0), Cities[0]);
                    numOfCities += 1;
                    Debug.Log("city spawned at " + x + "," + y);
                    spawns[i].transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
                    
                    i++;
                }
                else
                {
                    tileMap.SetTile(new Vector3Int(x, y, 0), beach[0]);
                    
                }

            }
            else if (E < 0.4)
            {

                int isCity = UnityEngine.Random.Range(0, 151);
                if ((isCity == 45) && (isCityMax == false))
                {
                    tileMap.SetTile(new Vector3Int(x, y, 0), Cities[0]);
                    numOfCities += 1;
                    Debug.Log("city spawned at " + x + "," + y);
                    spawns[i].transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
                    
                    i++;
                }
                else if (m < 0.5)
                {
                    tileMap.SetTile(new Vector3Int(x, y, 0), grass[0]);
                   
                }
                else if (m > 0.7)
                {
                    tileMap.SetTile(new Vector3Int(x, y, 0), swamp[0]);
                    
                }
                else
                {
                    tileMap.SetTile(new Vector3Int(x, y, 0), grass[0]);
                    
                }


            }

            else if (E < 0.5)
            {
                int isCity = UnityEngine.Random.Range(0, 200);
                if ((isCity == 45) && (isCityMax == false))
                {
                    tileMap.SetTile(new Vector3Int(x, y, 0), Cities[0]);
                    numOfCities += 1;
                    Debug.Log("city spawned at " + x + "," + y);
                    spawns[i].transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
                    
                    i++;
                }
                /* else if (m < 0.6)
                 {
                     //add jungle tile
                     tileMap.SetTile(new Vector3Int(x, y, 0), swamp[0]);
                 }*/
                else
                {
                    tileMap.SetTile(new Vector3Int(x, y, 0), forest[0]);
                    
                }

            }
            else if (E < 0.65)
            {
                int isCity = UnityEngine.Random.Range(0, 300);
                if ((isCity == 45) && (isCityMax == false))
                {
                    tileMap.SetTile(new Vector3Int(x, y, 0), Cities[0]);
                    numOfCities += 1;
                    Debug.Log("city spawned at " + x + "," + y);
                    spawns[i].transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
                   
                    i++;
                }
                else if (m < 0.5)
                {
                    tileMap.SetTile(new Vector3Int(x, y, 0), grass[0]);
                    
                }
                else if (m > 0.8)
                {
                    tileMap.SetTile(new Vector3Int(x, y, 0), swamp[0]);
                   
                }
                else
                {
                    tileMap.SetTile(new Vector3Int(x, y, 0), grass[0]);
                    ;
                }


            }
            else if (E < 0.8)
            {
                tileMap.SetTile(new Vector3Int(x, y, 0), desert[0]);
               
            }
            else if (E < 1.0)
            {
                tileMap.SetTile(new Vector3Int(x, y, 0), iceMountains[0]);
                
            } 
            else tileMap.SetTile(new Vector3Int(x, y, 0), debugSprite[0]);
            
        }
        
        float CalculateHeight(double nx, double ny)
        {

            float xCoord = (float)(nx / width * scale + offsetX);
            float yCoord = (float)(ny / height * scale + offsetY);
            return Mathf.PerlinNoise(xCoord, yCoord);
        }
        float CalculateMoisture(double x, double y)
        {

            float xCoord = (float)(x / width * scale + offsetX);
            float yCoord = (float)(y / height * scale + offsetY);
            return Mathf.PerlinNoise(xCoord, yCoord);
        }

        
    }

}
