using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Core : MonoBehaviour
{
    public GameObject arena, spaceship;
    public List<GameObject> playerSpawns;
    private int playerCount;

    private Transform spawnpoints;

    public void StartGame()
    {
        var mgm = MinigameManager.Instance;
        var sig = mgm.SignalR;
        playerCount = sig.Players.Count;


        Instantiate(arena);
        GameObject players = Instantiate(new GameObject("Players"));

        spawnpoints = arena.transform.Find("SpawnPoints");

        foreach (var player in sig.Players.Values)
        {
            for (int j = 0; j < spawnpoints.childCount; j++)
            {
                if (spawnpoints.GetChild(j).name.Contains((player.PlayerIndex + 1).ToString()))
                {
                    Transform transformChild = spawnpoints.GetChild(j).transform;

                    GameObject newPlayer = Instantiate(spaceship);
                    var playerControls = newPlayer.GetComponent<PlayerControls>();

                    player.OnButton0Press.AddListener(() => playerControls.Button0Pressed());
                    player.OnButton1Press.AddListener(() => playerControls.Button1Pressed());
                    player.OnButton2Press.AddListener(() => playerControls.Button2Pressed());


                    newPlayer.transform.position = transformChild.transform.position;
                    newPlayer.transform.rotation = transformChild.transform.rotation;

                    newPlayer.name = "Player" + (player.PlayerIndex + 1);
                    newPlayer.transform.SetParent(players.transform);
                }
            }

        }


        //Spel klaar
        //FindFirstObjectByType<GameFlow>().EndGame();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
