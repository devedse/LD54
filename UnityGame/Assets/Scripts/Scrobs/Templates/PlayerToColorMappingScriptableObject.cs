using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerToColorMappingScriptableObject", menuName = "ScriptableObject/Player Colors")]
public class PlayerToColorMappingScriptableObject : ScriptableObject
{
    public List<Color> Colors;
}
