using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public GameObject arena, spaceship, players;
    public List<GameObject> playerSpawns;
    private int playerCount;

    private Transform spawnpoints;

    public void StartGame()
    {
        var mgm = MinigameManager.Instance;
        var sig = mgm.SignalR;
        playerCount = sig.Players.Count;

        Instantiate(arena);
        players = Instantiate(new GameObject("Players"));

        spawnpoints = arena.transform.Find("SpawnPoints");

        foreach (var player in sig.Players.Values)
        {
            for (int j = 0; j < spawnpoints.childCount; j++)
            {
                if (spawnpoints.GetChild(j).name.Contains((player.PlayerIndex + 1).ToString()))
                {
                    // Create ships for all players.
                    Transform transformChild = spawnpoints.GetChild(j).transform;

                    GameObject newPlayer = Instantiate(spaceship);
                    var playerControls = newPlayer.GetComponent<PlayerControls>();
                    playerControls.pcplayer = player;

                    player.OnButton0Press.AddListener(() => playerControls.Button0Pressed());
                    player.OnButton1Press.AddListener(() => playerControls.Button1Pressed());
                    player.OnButton2Press.AddListener(() => playerControls.Button2Pressed());

                    player.OnButton0Release.AddListener(() => playerControls.Button0Released());
                    player.OnButton1Release.AddListener(() => playerControls.Button1Released());
                    player.OnButton2Release.AddListener(() => playerControls.Button2Released());

                    newPlayer.GetComponentInChildren<SpaceShipFiller>().SetProps(player);

                    newPlayer.transform.position = transformChild.transform.position;
                    newPlayer.transform.rotation = transformChild.transform.rotation;

                    newPlayer.name = "Player" + (player.PlayerIndex + 1);
                    newPlayer.transform.SetParent(players.transform);
                }
            }

        }


    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (players.transform.childCount == 1)
        {
            Debug.Log(players.transform.GetChild(0).name + " is Victorious!!");

            FindFirstObjectByType<GameFlow>().EndGame();
        }
    }
}
