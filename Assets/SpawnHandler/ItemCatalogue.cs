using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemCatalogue : MonoBehaviour
{
    public GameObject go;
    public byte gameplayWeight;
    public bool isLoopingEnabled;

    public ItemCatalogue(GameObject newGO, byte newGameplayWeight, bool newIsLoopingEnabled)
    {
        go = newGO;
        gameplayWeight = newGameplayWeight;
        isLoopingEnabled = newIsLoopingEnabled;
    }
    
}
