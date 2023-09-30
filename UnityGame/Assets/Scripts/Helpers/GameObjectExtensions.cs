using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public static class GameObjectExtensions
    {
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
    }
}
