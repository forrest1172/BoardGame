using UnityEngine;
using UnityEngine.Tilemaps;

public class ExampleClass : MonoBehaviour
{
    public TileBase[] tileArray;
    public BoundsInt area;
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(area.center, area.size);
    }
   public void GetArray()
    {
        
        Tilemap tilemap = GetComponent<Tilemap>();
        
        for (int i = 0; i < tileArray.Length; i++)
        {
            tileArray[i] = tilemap.GetTilesBlock(area)[i];
            print(tileArray[i]);
        }
    }
}
