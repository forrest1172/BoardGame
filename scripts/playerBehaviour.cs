using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Tilemaps;
using System;
using System.Linq;

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
    [SyncVar]
    public int myTurnNumber;

    public bool startSelected;
    public bool setOrders;
    public List<GameObject> Agents = new List<GameObject>();

    private Vector3 ResetCamera;
    private Vector3 Origin;
    private Vector3 Diference;
    private bool Drag = false;

    //maybe put on tilemap object
    public BoundsInt area;
    public GameObject playerRadius;

    public List<Vector3Int> tileNeighbors = new List<Vector3Int>();
    private int storedNumsCanMove;

    /*public Vector3Int[] leftTiles;
    public Vector3Int[] rightTiles;
    public Vector3Int[] UpTiles;
    public Vector3Int[] DownTiles;
    public Vector3Int[] upLeftTiles;
    public Vector3Int[] DownTiles;
    */
    private Tilemap map;


    void Start()
    {
        
    

    
        
        if (isLocalPlayer)
        {
            storedNumsCanMove = numCanMove;
            tilemap = GameObject.FindGameObjectWithTag("TileMap");
            manager = GameObject.FindGameObjectWithTag("GameManager");
            manager.GetComponent<mapgen>().GetSeed();
            manager.GetComponent<mapgen>().currentSeed = MySeedNumber;
            map = tilemap.GetComponent<Tilemap>();
            Agents.AddRange(GameObject.FindGameObjectsWithTag ("Agent"));
            Enabled[0].SetActive(true);
            GetComponent<playerStats>().Genstats();
            manager.GetComponent<Manager>().players.Add(this.gameObject);
            manager.GetComponent<Manager>().Ready();
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


        }

    }
    public void Move()
    {
        if (isLocalPlayer == true)
        {
            Enabled[0].SetActive(false);
            Enabled[1].SetActive(false);
            Enabled[2].SetActive(true);
            playerRadius.SetActive(true);
            GetWalkableTiles();
        }
        
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(area.center, area.size);
    }
    public void GetWalkableTiles()
    {
        tileNeighbors.Clear();
        
        if (numCanMove <= 0)
        {
            playerRadius.SetActive(false);
            //endTurn on current tile
            return;
        }
        else
        {
            area.position = new Vector3Int(Mathf.RoundToInt(transform.position.x - 1.5f), Mathf.RoundToInt(transform.position.y - 1.5f), 0);
            area.size = new Vector3Int(3, 3, 0);

            float x = transform.position.x;
            float y = transform.position.y - 0.5f;
            int X = (int)x;
            int Y = (int)y;



            
            for (int i = 0; i < 1; i++)
            {
                
                Vector3 left = new Vector3Int(X - (1 + i), Y, 0);
                tileNeighbors.Add(Vector3Int.FloorToInt(left));
                

                Vector3 right = new Vector3Int(X + (1 + i), Y, 0);
                tileNeighbors.Add(Vector3Int.FloorToInt(right));

                Vector3 up = new Vector3Int(X, Y + (1 + i), 0);
                tileNeighbors.Add(Vector3Int.FloorToInt(up));

                Vector3 down = new Vector3Int(X, Y - (1 + i), 0);
                tileNeighbors.Add(Vector3Int.FloorToInt(down));

                Vector3 upLeft = new Vector3Int(X - (1 + i), Y + (1 + i), 0);
                tileNeighbors.Add(Vector3Int.FloorToInt(upLeft));

                Vector3 downLeft = new Vector3Int(X - (1 + i), Y - (1 + i), 0);
                tileNeighbors.Add(Vector3Int.FloorToInt(downLeft));

                Vector3 upRight = new Vector3Int(X + (1 + i), Y + (1 + i), 0);
                tileNeighbors.Add(Vector3Int.FloorToInt(upRight));

                Vector3 downRight = new Vector3Int(X + (1 + i), Y - (1 + i), 0);
                tileNeighbors.Add(Vector3Int.FloorToInt(downRight));

            }
            
        }
        
        

    }
    
    public void EndTurn()
    {
        Enabled[2].SetActive(false);

        //restart turn
        Enabled[0].SetActive(true);
        numCanMove = storedNumsCanMove;
        float x = transform.position.x;
        float y = transform.position.y - 0.5f;
        int X = (int)x;
        int Y = (int)y;
        Vector3Int cp = new Vector3Int(X, Y, 0);
        TileBase endTile = map.GetTile(cp);
        manager.GetComponent<GenerateEncounter>().GenEncounter(endTile, this.gameObject);
        playerRadius.SetActive(false);

    }

    public void GetHome(string homeName)
    {
        
        Debug.Log("players home is " + homeName);
        home = homeName;
    }
    public void SetOrders()
    {
        setOrders = true;
    }

    void LateUpdate()
    {
        if (isLocalPlayer)
        {

            if(home == "")
            {
                manager.GetComponent<mapgen>().SpawnPlayer();
            }
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int tilePos = map.WorldToCell(pos);
                TileBase selectedTile = map.GetTile(tilePos);
                Debug.Log(selectedTile);
                if(setOrders == true)
                {
                    if (startSelected == false)
                    {
                        manager.GetComponent<Astar>().Initialize(tilePos);
                        startSelected = true;
                    }
                    else if (startSelected == true)
                    {
                        manager.GetComponent<Astar>().EndNode(tilePos);
                        foreach (var go in Agents)
                        {
                            go.GetComponent<Agent>().GetOrders(tilePos);
                        }
                        startSelected = false;
                        setOrders = false;
                    }
                }

               
                foreach(var tilesPositon in tileNeighbors.ToArray())
                {
                    
                    if (tilesPositon == tilePos)
                    {
                        if (selectedTile != manager.GetComponent<mapgen>().water[0])
                        {
                            numCanMove--;
                            transform.position = new Vector3(tilePos.x + 0.5f, tilePos.y + 0.5f, 0);
                        }
                        else if(tilesPositon != tilePos || selectedTile == manager.GetComponent<mapgen>().water[0])
                        {
                            Debug.Log("cannot move there");
                        }
                            

                        
                        GetWalkableTiles();
                    }
                }
                
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
