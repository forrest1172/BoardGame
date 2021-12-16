using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class MapSeed : NetworkBehaviour
{
    [SyncVar]
    public int GameSeed;

    // Start is called before the first frame update
    private void Start()
    {
        if (isServer)
        {
            GenSeed();
        }
    }

    // Update is called once per frame





    public void GenSeed()
    {

        int gs = UnityEngine.Random.Range(0, 9999);
        GameSeed = gs;

    }
}
    
