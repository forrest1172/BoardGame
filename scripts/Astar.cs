using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Astar : MonoBehaviour
{
    public Tilemap map;
    public Tilemap worldMap;

    private Node current;

    private HashSet<Node> openList;
    private HashSet<Node> closedList;

    private Vector3Int start;
    private Vector3Int goal;
    public TileBase[] tiles;

    private Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();

    public Stack<Vector3Int> path;

    public void Initialize(Vector3Int startTile)
    {
        start = startTile;
        current = GetNode(startTile);

        openList = new HashSet<Node>();
        closedList = new HashSet<Node>();

        openList.Add(current);
        
    }

    public void EndNode(Vector3Int endTile)
    {
        goal = endTile;
        //if goal is null return and do nothing
        while(openList.Count > 0 && path == null)
        {
            List<Node> neighbors = FindNeighbors(current.Position);
            ExamineNeighbors(neighbors, current);
            UpdateCurrentTile(ref current);

            path = GenPath(current);
        }

        if(path != null)
        {
            foreach (Vector3Int position in path)
            {
               if(position != goal)
                {
                    map.SetTile(position, tiles[0]);
                }
                
            }
        }
        
    }

    private List<Node> FindNeighbors(Vector3Int parentPos)
    {
        List<Node> neighbors = new List<Node>();

        for(int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int neighborPos = new Vector3Int(parentPos.x - x, parentPos.y - y, parentPos.z);

                if(y!= 0 || x != 0)
                {
                    if(neighborPos != start && worldMap.GetTile(neighborPos) && worldMap.GetTile(neighborPos) != GetComponent<mapgen>().water[0])
                    {
                        Node neighbor = GetNode(neighborPos);
                        neighbors.Add(neighbor);
                    }
                    
                }
            }
        }
        return neighbors;
    }

    private void ExamineNeighbors(List<Node> neighbors, Node current)
    {
        for(int i = 0;  i < neighbors.Count; i++)
        {
            Node neighbor = neighbors[i];
            int gScore = CalcGScore(neighbors[i].Position, current.Position);

            if (openList.Contains(neighbor))
            {
                if(current.G + gScore < neighbor.G)
                {
                    CalcValues(current, neighbor, gScore);
                }
            }
            else if (!closedList.Contains(neighbor))
            {
                CalcValues(current, neighbor, gScore);

                openList.Add(neighbor);
            }

        }
    }

    private void CalcValues(Node parent, Node neighbor, int cost)
    {
        neighbor.Parent = parent;

        neighbor.G = parent.G + cost;

        neighbor.H = ((Math.Abs(neighbor.Position.x - goal.x)) + (Math.Abs(neighbor.Position.y - goal.y)) * 10);

        neighbor.F = neighbor.G + neighbor.H;
    }
    private int CalcGScore(Vector3Int neighbor, Vector3Int current)
    {
        int gScore = 0;

        int x = current.x - neighbor.x;
        int y = current.y - neighbor.y;

        if(Math.Abs(x-y) % 2 == 1)
        {
            gScore = 10;
        }
        else
        {
            gScore = 14;
        }
        return gScore;
    }

    private void UpdateCurrentTile(ref Node current)
    {
        openList.Remove(current);
        closedList.Add(current);

        if(openList.Count > 0)
        {
            current = openList.OrderBy(x => x.F).First();
        }
    }

    private Node GetNode(Vector3Int position)
    {
        if (allNodes.ContainsKey(position))
        {
            return allNodes[position];

        }
        else
        {
            Node node = new Node(position);
            allNodes.Add(position, node);
            return node;
        }
    }

    private Stack<Vector3Int> GenPath(Node current)
    {
        if(current.Position == goal)
        {
            Stack<Vector3Int> finalPath = new Stack<Vector3Int>();
            

            while (current.Position != start)
            {
                finalPath.Push(current.Position);

                current = current.Parent;
            }
            
            return finalPath;
        }

        return null;
    }

    public void RestPath()
    {
        map.ClearAllTiles();
        allNodes.Clear();
        path = null;
        current = null;

    }
}
