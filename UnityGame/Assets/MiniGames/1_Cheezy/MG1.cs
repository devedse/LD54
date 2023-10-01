using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MG1 : MonoBehaviour
{
    public GameObject cheese;
    public GameObject allChildObjectStuff;

    private int[] cheeseCountPerRound = new int[] { 5, 4, 7, 8, 10, 5 };
    private int currentRound = 0;
    private int playerCount = 3;

    public Material[] colorMaterials;

    private List<int> whatColorsThisRound = new List<int>();
    private List<int> playerCurRound = new List<int>();

    private const float offsetYPerCheese = 1.1f;
    private const float leftX = -8f;
    private const float rightX = 8f;
    private const float cheeseWidth = 4f;
    private const float cheeseGap = 1f;

    private float currentScale = 1f;


    private Dictionary<int, bool> PlayerOnCooldown = new Dictionary<int, bool>();

    public GameObject CooldownUI;

    private int playersFinished = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void StartGame()
    {
        var mgm = MinigameManager.Instance;
        var sig = mgm.SignalR;
        playerCount = sig.Players.Count;

        foreach (var player in sig.Players.Values)
        {
            player.OnButton0Press.AddListener(() => PlayerButtonPress(player.PlayerIndex, 0));
            player.OnButton1Press.AddListener(() => PlayerButtonPress(player.PlayerIndex, 1));
            player.OnButton2Press.AddListener(() => PlayerButtonPress(player.PlayerIndex, 2));
        }

        NewRound();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform player in allChildObjectStuff.transform)
        {
            var playerScore = playerCurRound[int.Parse(player.name)];
            //Lerp local position using offset per cheese * playerScore
            var lerpY = Mathf.Lerp(player.localPosition.y, (offsetYPerCheese * currentScale) * -playerScore, Time.deltaTime * 5f);
            player.localPosition = new Vector3(player.localPosition.x, lerpY, player.localPosition.z);
        }
    }

    public IEnumerator FakeButtons()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            var randomPlayer = Random.Range(0, playerCount);
            var randomButton = Random.Range(0, 3);
            var gameIsOver = PlayerButtonPress(randomPlayer, randomButton);
            if (gameIsOver)
            {
                break;
            }
        }
    }



    public IEnumerator CooldownPlayer(int player)
    {
        var start = Time.time;

        var gimmePos = PlayerPositioner.DistributePlayers(playerCount, leftX, rightX, cheeseWidth, cheeseGap);
        PlayerOnCooldown[player] = true;

        var cdUI = GameObject.Instantiate(CooldownUI);
        cdUI.transform.localPosition = new Vector3(gimmePos.positions[player], -2.5f, -gimmePos.scale - 0.01f);
        var meshRender = cdUI.GetComponentInChildren<MeshRenderer>();

        var madTexture = MinigameManager.Instance?.SignalR?.GetPlayerByNumber(player)?.PlayerMad?.texture;
        if (madTexture != null)
        {
            meshRender.material.mainTexture = madTexture;
        }

        while (Time.time - start < 1f)
        {
            var diff = Time.time - start;
            cdUI.transform.localScale = Vector3.one * 4 * gimmePos.scale * (Mathf.Sin(diff * 4f) / 10f + 1f);
            yield return null;
        }
        Destroy(cdUI);
        PlayerOnCooldown[player] = false;
    }

    public bool PlayerButtonPress(int player, int button)
    {
        if (PlayerOnCooldown.ContainsKey(player) && PlayerOnCooldown[player])
        {
            return false;
        }

        if (playerCurRound[player] >= cheeseCountPerRound[currentRound])
        {
            //Round already over for player
        }
        else
        {
            var currentRoundForPlayer = playerCurRound[player];
            var currentCheeseTopress = whatColorsThisRound[currentRoundForPlayer];

            if (currentCheeseTopress == button)
            {
                //Correct
                playerCurRound[player]++;

                var playerUI = allChildObjectStuff.GetChildGameObjectByName(player.ToString());
                var specificCheese = playerUI.GetChildGameObjectByName(currentRoundForPlayer.ToString());
                Destroy(specificCheese);


                if (playerCurRound[player] >= cheeseCountPerRound[currentRound])
                {
                    //Player won
                    //Debug.Log("Player " + player + " won this round");
                    MinigameManager.Instance.SignalR.GetPlayerByNumber(player).ChangeScore(3 - playersFinished);
                    playersFinished++;
                }
            }
            else
            {
                //Wrong
                //Debug.Log("Player " + player + " pressed wrong button");
                MinigameManager.Instance.SignalR.GetPlayerByNumber(player).ChangeScore(0);

                StartCoroutine(CooldownPlayer(player));
            }
        }

        if (playerCurRound.All(t => t >= cheeseCountPerRound[currentRound]))
        {
            currentRound++;
            //All players won
            Debug.Log("All players won this round");

            if (currentRound >= cheeseCountPerRound.Length)
            {
                FindFirstObjectByType<GameFlow>().EndGame();
                //return true;
            }
            else
            {
                NewRound();
            }
        }
        return false;
    }

    public void NewRound()
    {
        playersFinished = 0;

        var scaleAlgorithmThingy = PlayerPositioner.DistributePlayers(playerCount, leftX, rightX, cheeseWidth, cheeseGap);
        currentScale = scaleAlgorithmThingy.scale;


        whatColorsThisRound = new List<int>();
        playerCurRound = new List<int>();

        allChildObjectStuff.RemoveAllChildObjects();


        var rootPlayerObjects = new List<GameObject>();
        for (int i = 0; i < playerCount; i++)
        {
            var ga = new GameObject(i.ToString());
            ga.transform.parent = allChildObjectStuff.transform;
            ga.transform.localPosition = new Vector3(scaleAlgorithmThingy.positions[i], 10, 0);
            ga.transform.localScale = new Vector3(scaleAlgorithmThingy.scale, scaleAlgorithmThingy.scale, scaleAlgorithmThingy.scale);
            rootPlayerObjects.Add(ga);
            playerCurRound.Add(0);
        }


        for (int y = 0; y < cheeseCountPerRound[currentRound]; y++)
        {
            var colorThisCheese = Random.Range(0, colorMaterials.Length);
            whatColorsThisRound.Add(colorThisCheese);
            var randomColor = colorMaterials[colorThisCheese];

            for (int i = 0; i < playerCount; i++)
            {
                var rootPlayerTransform = rootPlayerObjects[i].transform;
                var ga = GameObject.Instantiate(cheese, rootPlayerTransform);
                ga.transform.localPosition = new Vector3(0, offsetYPerCheese * y, 0);
                ga.name = y.ToString();
                // Assuming the cheese has a Renderer component
                Renderer renderer = ga.GetComponentInChildren<MeshRenderer>();
                var blah = ga.gameObject.GetComponentInChildren<Renderer>();
                if (renderer != null)
                {
                    renderer.material = randomColor;
                }
            }
        }
    }
}
