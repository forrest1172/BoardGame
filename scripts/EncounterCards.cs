using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Encounter", menuName = "Encounters")]
public class EncounterCards : ScriptableObject
{
    public string cardName;
    [SerializeField]
    public static TextAsset jsonText;
    public string mainText = jsonText.text;
    public Sprite artwork;

    public int cardNumber;

    








}
