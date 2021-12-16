using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

public class spawnPointData : MonoBehaviour
{
    //public List<string> savedNames = new List<string>();
    public GameObject cityName;
    private Tilemap tm;
    private TileBase ct;
    public string nameString;
    public mapgen map;
    public bool isPort;

    public string[] names = new string[] { "Kliemont", "Abochester", "Zleving", "Brehpus", "Flodence", "New Jonathstead","Old Andyport","Wonksville","Ol' ElijahBerg","Rustlestead","Lynxington",
        "joshester","Ericester","Yleles","Shork","Iglurgh","Onioport","Aresburgh","Lamouth","Glaasas","Evresmouth","Sodon","Edorith","Oliswell","Rugas","Neading","Glagan","Ylenard","Ifloria","Qasas",
        "Primfast","Dathe", "Erygate","Yhanton","Flance","Braalas","Ilaburn","Glondale","Chemore","Rolk","Edosas","Aqroni","Hathe","Estermont","Idoset","Blecgate","Slocaster","Ardrith","Ordfield",
        "Zrudale","Bluoxpus","Agosphia","Dirtin","Mukstead","Druxbury","Giewood","Fliakshire","Olisdale","Yada","Drokdale","Flicpool" };
        /*"","","","","","","","","","","","","",
        "","","","","","","","","","",""};*/
    
    void Start()
    {
       GameObject tileGo = GameObject.FindGameObjectWithTag("TileMap");
        tm = tileGo.GetComponent<Tilemap>();
        
        ct  = tm.GetTile(new Vector3Int(Mathf.RoundToInt(this.transform.position.x - 0.5f), Mathf.RoundToInt(this.transform.position.y - 0.5f), 0));
       
    }

    public void SetName()
    {
        int i = Random.Range(0, names.Length);
        nameString = names[i];
        if (ct.name == "waterDeep01")
        {
            cityName.GetComponent<TextMesh>().text = "";
            return;
        }
        else if (isPort == true)
        {
            gameObject.name = "Port " + nameString;

            cityName.GetComponent<TextMesh>().text = "Port " + names[i];
        }
        else
        {
            
            gameObject.name = nameString;
           
            cityName.GetComponent<TextMesh>().text = names[i];
            
        }
         
    }
    public void GetNeighbors()
    {
        float x = transform.position.x;
        float y = transform.position.y - 0.5f;
        int X = (int)x;
        int Y = (int)y;
        Vector3Int leftTile = new Vector3Int(X - 1, Y, 0);
        Vector3Int rightTile = new Vector3Int(X + 1, Y, 0);
        Vector3Int UpTile = new Vector3Int(X, Y + 1, 0);
        Vector3Int DownTile = new Vector3Int(X, Y - 1, 0);
        
        if (tm.GetTile(leftTile) == map.water[0])
        {

            isPort = true;
        }

        else if (tm.GetTile(rightTile) == map.water[0])
        {
            isPort = true;
        }

        else if (tm.GetTile(UpTile) == map.water[0])
        {
            isPort = true;
        }

        else if (tm.GetTile(DownTile) == map.water[0])
        {
            isPort = true;
        }
        else isPort = false;
        SetName();
    }
}
