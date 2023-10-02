using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Core : MonoBehaviour
{
    public GameObject arena;
    public BoxCollider spawnArea;
    public GameObject spaceshipPrefab;

    public GameObject spawnPointRoot;


    private GameObject players;

    public void StartGame()
    {
        var mgm = MinigameManager.Instance;
        var sig = mgm.SignalR;

        Instantiate(arena);
        players = Instantiate(new GameObject("Players"));

        var spawnpoints = spawnPointRoot.transform.GetChildren().ToList();

        foreach (var player in sig.Players.Values)
        {
            Debug.Log(spawnArea.bounds.min.x + " " + spawnArea.bounds.max.x);

            Vector3? spawnPosition = null;
            if (player.PlayerIndex < spawnpoints.Count)
            {
                spawnPosition = spawnpoints[player.PlayerIndex].localPosition;
            }
            else
            {
                //random position inside spawn area
                var x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
                var y = spawnpoints[0].localPosition.y;
                var z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);

            }


            GameObject newPlayer = Instantiate(spaceshipPrefab, players.transform);
            var playerControls = newPlayer.GetComponent<PlayerControls>();
            playerControls.pcplayer = player;

            player.OnButton0Press.AddListener(() => playerControls.Button0Pressed());
            player.OnButton1Press.AddListener(() => playerControls.Button1Pressed());
            player.OnButton2Press.AddListener(() => playerControls.Button2Pressed());

            player.OnButton0Release.AddListener(() => playerControls.Button0Released());
            player.OnButton1Release.AddListener(() => playerControls.Button1Released());
            player.OnButton2Release.AddListener(() => playerControls.Button2Released());

            newPlayer.GetComponentInChildren<SpaceShipFiller>().SetProps(player);

            newPlayer.transform.localPosition = spawnPosition.Value;
            newPlayer.transform.LookAt(transform);

            newPlayer.name = "Player" + (player.PlayerIndex + 1);

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
