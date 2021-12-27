using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Agent : MonoBehaviour
{
    public Astar pathfinder;
    public float speed = 2;
    private List<Vector3Int> pathPos = new List<Vector3Int>();
    public Tilemap world;
    private mapgen map;
    public Vector3Int target;
    
    // Start is called before the first frame update
    void Start()
    {
        pathfinder = GameObject.Find("GameManager").GetComponent<Astar>();
        map = GameObject.Find("GameManager").GetComponent<mapgen>();
        world = GameObject.FindGameObjectWithTag("TileMap").GetComponent<Tilemap>();
    }

    public void GetOrders(Vector3Int Target)
    {
        target = Target;
        foreach (Vector3Int position in pathfinder.path)
        {
            pathPos.Add(position);
        }
        StartCoroutine(agentSpeed());

    }
    IEnumerator agentSpeed()
    {
        WaitForSeconds wait = new WaitForSeconds(speed);

        for (int i = 0; i < pathPos.Count; i++)
        {
            TileBase currentTile = world.GetTile(pathPos[i]);
            if (currentTile != map.swamp[0])
            {
                speed = 0.2f;
                transform.position = new Vector3(pathPos[i].x + 0.5f, pathPos[i].y + 0.5f, 0);
            }
            else speed = 2;
            
             
            transform.position = new Vector3(pathPos[i].x + 0.5f, pathPos[i].y + 0.5f, 0);
            yield return wait = new WaitForSeconds(speed);

        }
        if(target == Vector3Int.FloorToInt(transform.position))
        {
            pathfinder.RestPath();
            pathPos.Clear();
        }
    }
    //update if target is null find target
    //
}
    