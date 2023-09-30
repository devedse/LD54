using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerImagesScriptableObject", menuName = "ScriptableObject/Player Images Collection")]
public class PlayerImagesScriptableObject : ScriptableObject
{
    public List<PlayerImageScriptableObject> Images;
}
