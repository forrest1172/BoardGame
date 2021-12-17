using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Tilemaps;
using System;

public class playerBehaviour : NetworkBehaviour
{
    public int numCanMove;
    public int health;
    public int scrap;
    public GameObject manager;
    public GameObject[] Enabled;
    public int MySeedNumber;
    public Camera cam;
    public GameObject tilemap;
    [SyncVar]
    public string home = "";

    private Vector3 ResetCamera;
    private Vector3 Origin;
    private Vector3 Diference;
    private bool Drag = false;
    
    public List leftTiles = new List<Vector3>();
    public List rightTiles = new List<Vector3>();
    public List upTiles = new List<Vector3>();
    public List downTiles = new List<Vector3>();
    
    //public Vector3Int[] leftTiles;
    //public Vector3Int[] rightTiles;
    //public Vector3Int[] UpTiles;
    //public Vector3Int[] DownTiles;
    
    

    void Start()
    {
        
            
        tilemap = GameObject.FindGameObjectWithTag("TileMap");
        manager = GameObject.FindGameObjectWithTag("GameManager");
        manager.GetComponent<mapgen>().GetSeed();
        manager.GetComponent<mapgen>().currentSeed = MySeedNumber;

        if (isLocalPlayer)
        {
            Enabled[0].SetActive(true);
        }
        if (!isLocalPlayer)
        {
            cam.enabled = false;
            Enabled[0].SetActive(false);
        }
    }
   public void Action()
    {
        if (isLocalPlayer)
        {

            Enabled[0].SetActive(false);
            Enabled[1].SetActive(true);
            Enabled[2].SetActive(true);
            Enabled[3].SetActive(true);

        }

    }
    public void Move()
    {
        if (isLocalPlayer == true)
        {
            Enabled[0].SetActive(false);
            Enabled[1].SetActive(false);
            Enabled[2].SetActive(false);
            Enabled[3].SetActive(false);
            Enabled[4].SetActive(true);
            GetWalkableTiles();
        }
        
    }

    public void GetWalkableTiles()
    {
        float x = transform.position.x;
        float y = transform.position.y - 0.5f;
        int X = (int)x;
        int Y = (int)y;
        for(int i = 0; i < numCanMove; i++)
        {
            Vector3 left = new Vector3Int(X - (1 + i), Y, 0);
            leftTiles.Add(left);
            
            Vector3 right = new Vector3Int(X + (1 + i), Y, 0);
            rightTiles.Add(right);
            
            Vector3 up = new Vector3Int(X, Y + (1 + i), 0);
            upTiles.Add(up);
            
            Vector3 down = new Vector3Int(X, Y - (1 + i), 0);
            downTiles.Add(down);


        }
        

    }
    public void Up()
    {
        if (isLocalPlayer == true)
        {
            float x = transform.position.x;
            float y = transform.position.y - 0.5f;
            int X = (int)x;
            int Y = (int)y;
            Vector3Int cp = new Vector3Int(X ,Y ,0);
            Vector3Int np = new Vector3Int(X, Y + 1, 0);
            TileBase nt = tilemap.GetComponent<Tilemap>().GetTile(np);
            if (nt != manager.GetComponent<mapgen>().water[0])
            {
                this.transform.position = new Vector3(x, y + 1, 0);
                leftTiles.Clear();
                rightTiles.Clear();
                upTiles.Clear();
                downTiles.Clear();

            }

            else return;

            
           
            
            
        }


    }
    public void Down()
    {
        if (isLocalPlayer == true)
        {
            float x = transform.position.x;
            float y = transform.position.y;
            int X = (int)x;
            int Y = (int)y;
            Vector3Int cp = new Vector3Int(X, Y, 0);
            Vector3Int np = new Vector3Int(X, Y - 1, 0);
            TileBase nt = tilemap.GetComponent<Tilemap>().GetTile(np);
            if (nt != manager.GetComponent<mapgen>().water[0])
            {
                transform.position = new Vector3(x, y - 1, 0);
                leftTiles.Clear();
                rightTiles.Clear();
                upTiles.Clear();
                downTiles.Clear();

            }

            else return;
        }


    }
    public void Left()
    {
        if (isLocalPlayer == true)
        {
            float x = transform.position.x;
            float y = transform.position.y;
            int X = (int)x;
            int Y = (int)y;
            Vector3Int cp = new Vector3Int(X, Y, 0);
            Vector3Int np = new Vector3Int(X - 1, Y, 0);
            TileBase nt = tilemap.GetComponent<Tilemap>().GetTile(np);
            if (nt != manager.GetComponent<mapgen>().water[0])
            {
                GetComponent<SpriteRenderer>().flipX = true;
                transform.position = new Vector3(x - 1, y, 0);
                manager.GetComponent<GenerateEncounter>().GenEncounter(nt);
                leftTiles.Clear();
                rightTiles.Clear();
                upTiles.Clear();
                downTiles.Clear();

            }

            else return;

        }


    }
    public void Right()
    {
        if (isLocalPlayer == true)
        {
            float x = transform.position.x;
            float y = transform.position.y;
            int X = (int)x;
            int Y = (int)y;
            Vector3Int cp = new Vector3Int(X, Y, 0);
            Vector3Int np = new Vector3Int(X + 1, Y, 0);
            TileBase nt = tilemap.GetComponent<Tilemap>().GetTile(np);
            if (nt != manager.GetComponent<mapgen>().water[0])
            {
                GetComponent<SpriteRenderer>().flipX = false;
                transform.position = new Vector3(x + 1, y, 0);
                leftTiles.Clear();
                rightTiles.Clear();
                upTiles.Clear();
                downTiles.Clear();

            }

            else return;
        }


    }

    public void GetHome(string homeName)
    {
        
        Debug.Log("players home is " + homeName);
        home = homeName;
    }

    void LateUpdate()
    {
        if (isLocalPlayer)
        {

            if(home == "")
            {
                manager.GetComponent<mapgen>().SpawnPlayer();
            }
            if (Input.GetMouseButton(0))
            {

                Diference = (cam.ScreenToWorldPoint(Input.mousePosition)) - cam.transform.position;
                if (Drag == false)
                {
                    Drag = true;
                    Origin = cam.ScreenToWorldPoint(Input.mousePosition);
                }
            }
            else
            {
                Drag = false;
            }
            if (Drag == true)
            {
                cam.transform.position = Origin - Diference;
            }
            //RESET CAMERA TO STARTING POSITION WITH RIGHT CLICK
            if (Input.GetMouseButton(1))
            {
                cam.transform.position = ResetCamera;
                cam.orthographicSize = 6;
            }
            ResetCamera = new Vector3(this.transform.position.x, this.transform.position.y, -10);
            
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                cam.orthographicSize++;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                cam.orthographicSize--;
            }

            

        }
    }
    
}
