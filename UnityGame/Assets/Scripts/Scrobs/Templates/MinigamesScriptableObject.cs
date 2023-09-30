using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinigamesScriptableObject", menuName = "ScriptableObject/Minigames Collection")]
public class MinigamesScriptableObject : ScriptableObject
{
    public List<MinigameScriptableObject> Minigames;
}
