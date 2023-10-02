using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
    static public IEnumerable<Transform> GetChildren(this Transform ga)
    {
        foreach (Transform child in ga.transform)
        {
            yield return child;
        }
    }

    static public GameObject GetChildGameObjectByName(this GameObject fromGameObject, string name)
    {
        foreach (Transform child in fromGameObject.transform)
        {
            if (child.name == name)
            {
                return child.gameObject;
            }
        }
        return null;
    }

    static public GameObject RemoveAllChildObjects(this GameObject gameObject)
    {
        foreach (Transform child in gameObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        return gameObject;
    }
}
