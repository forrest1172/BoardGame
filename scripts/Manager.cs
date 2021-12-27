using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror;

public class Manager : NetworkBehaviour
{
    public List<GameObject> players = new List<GameObject>();
    [SyncVar]
    public int turnCounter;
    [SyncVar]
    public int currentTurn;
    [SyncVar]
    public int startTurn;

    public void Ready()
    {
        for(int i = 0; i < players.Count; i++)
        {
            players[i].GetComponent<playerBehaviour>().myTurnNumber = startTurn;
            startTurn++;
        }
    }

    public void LateUpdate()
    {
       
    }

}
