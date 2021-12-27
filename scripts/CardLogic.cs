using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardLogic : MonoBehaviour
{
    public EncounterCards card;
    public TextMeshProUGUI main;

    public GameObject artwork;

    // Start is called before the first frame update
    void Start()
    {
       main.text = card.mainText;
       artwork.GetComponent<Image>().sprite = card.artwork;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
