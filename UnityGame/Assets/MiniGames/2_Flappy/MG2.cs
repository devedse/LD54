using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MG2 : MonoBehaviour
{
    public GameObject Flap;
    public GameObject Building;

    private List<GameObject> Flappers = new List<GameObject>();
    private List<GameObject> Buildings = new List<GameObject>();

    private int playerCount = 2;

    public Dictionary<int, bool> LeftButtonPressed = new Dictionary<int, bool>();
    public Dictionary<int, bool> RightButtonPressed = new Dictionary<int, bool>();

    //8 Colors
    private Color[] PlayerColors = new Color[8] {
        Color.red, Color.blue, Color.green, Color.yellow,
        Color.cyan, Color.magenta, Color.grey, Color.white };

    private float StartTime;

    private float LastScoreTime;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void StartGame()
    {
        StartTime = Time.timeSinceLevelLoad;
        LastScoreTime = Time.timeSinceLevelLoad;
        Physics.gravity = new Vector3(0, -50, 0);

        var mgm = MinigameManager.Instance;
        var sig = mgm.SignalR;
        playerCount = sig.Players.Count;

        foreach (var player in sig.Players.Values)
        {
            player.OnButton0Press.AddListener(() => PlayerButtonPress(player.PlayerIndex, 0, true));
            player.OnButton1Press.AddListener(() => PlayerButtonPress(player.PlayerIndex, 1, true));
            player.OnButton2Press.AddListener(() => PlayerButtonPress(player.PlayerIndex, 2, true));
            player.OnButton0Release.AddListener(() => PlayerButtonPress(player.PlayerIndex, 0, false));
            player.OnButton1Release.AddListener(() => PlayerButtonPress(player.PlayerIndex, 1, false));
            player.OnButton2Release.AddListener(() => PlayerButtonPress(player.PlayerIndex, 2, false));
        }

        for (int i = 0; i < playerCount; i++)
        {
            GameObject playerFlap = GameObject.Instantiate(Flap, this.transform);
            playerFlap.name = i.ToString();
            playerFlap.transform.position = new Vector3(-5, 3 - (i * 2f), 0);

            var spaceShipFiller = playerFlap.GetComponent<SpaceShipFiller>();
            var player = MinigameManager.Instance.SignalR.GetPlayerByNumber(i);
            spaceShipFiller.SetProps(player);

            //var meshRenderer = playerFlap.GetComponent<MeshRenderer>();

            //meshRenderer.material.color = PlayerColors[i];

            Flappers.Add(playerFlap);
        }

        //StartCoroutine(FakeButtons());
    }

    public bool PlayerButtonPress(int player, int button, bool pressed)
    {
        var curFlap = Flappers[player];
        var rigidBody = curFlap.GetComponent<Rigidbody>();
        if (button == 1 && pressed)
        {
            rigidBody.velocity = new Vector3(0, 15, 0);
        }
        else if (button == 0)
        {
            LeftButtonPressed[player] = pressed;
        }
        else if (button == 2)
        {
            RightButtonPressed[player] = pressed;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        //Start with a speed of 1f and increase slowly over time up (in 30 seconds) to 5f

        var buildingSpeed = Mathf.Lerp(1f, 50f, (Time.timeSinceLevelLoad - StartTime) / 120f);



        foreach (var building in Buildings)
        {
            building.transform.localPosition -= new Vector3(buildingSpeed * Time.deltaTime, 0, 0);
        }

        if (Buildings.Count == 0 || Buildings.Max(t => t.transform.localPosition.x) < 10)
        {
            var newBuildingBot = GameObject.Instantiate(Building, this.transform);
            var newBuildingTop = GameObject.Instantiate(Building, this.transform);
            var randomY = Random.Range(-7.5f, -3.2f);

            int startX = 15;
            int gap = 5;

            newBuildingBot.transform.localPosition = new Vector3(startX, randomY, 0);
            newBuildingTop.transform.localEulerAngles = new Vector3(0, 0, 180);

            newBuildingTop.transform.localPosition = new Vector3(startX, randomY + Building.transform.localScale.y + gap, 0);

            var topRenderer = newBuildingBot.GetComponent<MeshRenderer>();
            topRenderer.material.color = PlayerColors[Random.Range(0, PlayerColors.Length)];
            var bottomRenderer = newBuildingTop.GetComponent<MeshRenderer>();
            bottomRenderer.material.color = PlayerColors[Random.Range(0, PlayerColors.Length)];
            Buildings.Add(newBuildingBot);
            Buildings.Add(newBuildingTop);
        }

        var toRemove = Buildings.Where(t => t.transform.localPosition.x < -10).ToList();

        foreach (var toRem in toRemove)
        {
            Buildings.Remove(toRem);
            GameObject.Destroy(toRem);
        }

        var playersToRemove = new List<GameObject>();

        bool shouldGiveScore = false;

        if (LastScoreTime + 1f < Time.timeSinceLevelLoad)
        {
            shouldGiveScore = true;
            LastScoreTime = Time.timeSinceLevelLoad;
        }

        foreach (var flap in Flappers)
        {
            var playerNumber = int.Parse(flap.name);

            if (LeftButtonPressed.ContainsKey(playerNumber) && LeftButtonPressed[playerNumber])
            {
                flap.transform.localPosition = new Vector3(flap.transform.localPosition.x - (5f * Time.deltaTime), flap.transform.localPosition.y, flap.transform.localPosition.z);
            }
            if (RightButtonPressed.ContainsKey(playerNumber) && RightButtonPressed[playerNumber])
            {
                flap.transform.localPosition = new Vector3(flap.transform.localPosition.x + (5f * Time.deltaTime), flap.transform.localPosition.y, flap.transform.localPosition.z);
            }

            if (flap.transform.localPosition.y < -3.5f)
            {
                PlayerButtonPress(playerNumber, 1, true);
            }

            var fpos = flap.transform.position;
            var fscale = flap.transform.localScale;
            var fbounds = new Bounds(fpos, fscale);

            bool dead = false;

            foreach (var building in Buildings)
            {
                var bpos = building.transform.position;
                var bscale = building.transform.localScale;
                var bbounds = new Bounds(bpos, bscale);

                if (fbounds.Intersects(bbounds))
                {
                    MinigameManager.Instance.SignalR.GetPlayerByNumber(playerNumber).ChangeScore(0);

                    playersToRemove.Add(flap);
                    dead = true;
                }
            }

            if (!dead && shouldGiveScore)
            {
                MinigameManager.Instance.SignalR.GetPlayerByNumber(playerNumber).ChangeScore(1);
            }
        }

        foreach (var flap in playersToRemove)
        {
            GameObject.Destroy(flap);
            Flappers.Remove(flap);
        }

        if (Flappers.Count == 0)
        {
            //Game over
            FindFirstObjectByType<GameFlow>().EndGame();
            return;
        }
    }
}
