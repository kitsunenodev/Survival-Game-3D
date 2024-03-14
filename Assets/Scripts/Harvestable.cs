using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvestable : MonoBehaviour
{
    public Resource[] harvestableLoot;

    [Header("Option")] 
    public Tool tool;
    public bool disableKinematicOnHarvest;
    public float destroyDelay;

}

[System.Serializable]
public class Resource
{
    public ItemData itemData;

    [Range(0,100)]
    public int dropProbability;
}

public enum Tool
{
    Pickaxe,
    Axe
}
