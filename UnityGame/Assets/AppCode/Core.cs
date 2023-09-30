using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Core : MonoBehaviour
{
    public GameObject arena, spaceship;
    public List<GameObject> playerSpawns;
    public int playerCount;

    private Transform spawnpoints;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(arena);
        GameObject players = Instantiate(new GameObject("Players"));

        spawnpoints = arena.transform.Find("SpawnPoints");

        for (int i = 0; i < playerCount; i++)
        {
            for (int j = 0; j < spawnpoints.childCount; j++)
            {
                if (spawnpoints.GetChild(j).name.Contains((i + 1).ToString()))
                {
                    Transform transformChild = spawnpoints.GetChild(j).transform;

                    GameObject newPlayer = Instantiate(spaceship);
                    newPlayer.transform.position = transformChild.transform.position;
                    newPlayer.transform.rotation = transformChild.transform.rotation;

                    newPlayer.name = "Player" + (i + 1);
                    newPlayer.transform.SetParent(players.transform);
                }
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
