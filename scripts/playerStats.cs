using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class playerStats : NetworkBehaviour
{
    [SyncVar]
    public int hp;
    public float speed;
    public float attack;
    public float defense;
    public float spAttack;
    public float spDefense;

    public string[] skills = new string[2];


    
    public void Genstats()
    {
        
            speed = Random.Range(1f, 10f);
            defense = Random.Range(1, 10);
            attack = Random.Range(1, 10);
            spAttack = Random.Range(1, 10);
            spDefense = Random.Range(1, 10);
            hp = Mathf.RoundToInt(defense + spDefense + 10 - speed + (spAttack + attack * 0.5f));
        
        
    }
}

    
