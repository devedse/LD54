using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipModuleScriptableObject", menuName = "ScriptableObject/Ship modules")]
public class ShipModuleScriptableObject : ScriptableObject
{
    public List<GameObject> AllShipModules;
    public List<GameObject> Positives;
    public List<GameObject> Negatives;
}
