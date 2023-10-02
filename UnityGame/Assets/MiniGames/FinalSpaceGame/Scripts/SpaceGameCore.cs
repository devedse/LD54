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
        foreach (var playerControls in players.GetComponentsInChildren<PlayerControls>())
        {
            Debug.Log($"Enabling player: {playerControls.gameObject.name}");
            playerControls.enabled = true;

            var player = playerControls.pcplayer;

            player.OnButton0Press.AddListener(() => playerControls.Button0Pressed());
            player.OnButton1Press.AddListener(() => playerControls.Button1Pressed());
            player.OnButton2Press.AddListener(() => playerControls.Button2Pressed());

            player.OnButton0Release.AddListener(() => playerControls.Button0Released());
            player.OnButton1Release.AddListener(() => playerControls.Button1Released());
            player.OnButton2Release.AddListener(() => playerControls.Button2Released());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        players = Instantiate(new GameObject("Players"));

        var spawnpoints = spawnPointRoot.transform.GetChildren().ToList();

        foreach (var player in MinigameManager.Instance.SignalR.Players.Values)
        {
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

                spawnPosition = new Vector3(x, y, z);
            }

            GameObject newPlayer = Instantiate(spaceshipPrefab, players.transform);
            var playerControls = newPlayer.GetComponent<PlayerControls>();
            playerControls.enabled = false;
            playerControls.pcplayer = player;

            newPlayer.GetComponentInChildren<SpaceShipFiller>().SetProps(player);

            newPlayer.transform.localPosition = spawnPosition.Value;
            newPlayer.transform.LookAt(transform);

            newPlayer.name = "Player" + (player.PlayerIndex + 1);
        }
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
