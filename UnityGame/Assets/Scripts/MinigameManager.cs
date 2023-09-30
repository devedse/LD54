using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour
{
    public GameObject ScoreCanvas;
    public IngameScoreScreen ScoreScreen;

    private static MinigameManager _instance;

    public static MinigameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var editorOnlyHackForInstanceWorkStuff = new GameObject();


                //So that shit works in Editor as well
                _instance = editorOnlyHackForInstanceWorkStuff.AddComponent<MinigameManager>();
                _instance.SignalR = editorOnlyHackForInstanceWorkStuff.AddComponent<SignalRTest>();

                for (int i = 0; i < 2; i++)
                {
                    var keyboardPlayerControllerGA = new GameObject();
                    keyboardPlayerControllerGA.name = $"KeboardPlayerController{i}";
                    var pc = keyboardPlayerControllerGA.AddComponent<PC>();
                    pc.PlayerIndex = i;

                    if (i == 0)
                    {
                        pc.PlayerName = PC.KeyboardZXCPlayerName;
                        pc.ListenToKeyboardZXC = true;
                    }
                    else if (i == 1)
                    {
                        pc.PlayerName = PC.KeyboardArrowKeysPlayerName;
                        pc.ListenToKeyboardArrowKeys = true;
                    }


                    var assettebandje = AssetDatabase.FindAssets("PlayerImagesScriptableObject").OrderBy(x => x).Select(x => AssetDatabase.GUIDToAssetPath(x)).Where(x => x.Contains("PlayerImagesScriptableObject.asset")).ToList();
                    var imgScrobs = AssetDatabase.LoadAssetAtPath<PlayerImagesScriptableObject>(assettebandje[0]).Images;
                    var imgScrob = imgScrobs[UnityEngine.Random.Range(0, imgScrobs.Count)];

                    pc.PlayerImage = imgScrob.ImageIdle;
                    pc.PlayerMad = imgScrob.ImageSad;
                    pc.PlayerHappy = imgScrob.ImageWin;
                    _instance.SignalR.Players.Add(i.ToString(), pc);
                }

                var miniGames = AssetDatabase.FindAssets("Minigames").OrderBy(x => x).Select(x => AssetDatabase.GUIDToAssetPath(x)).Where(x => x.Contains("Minigames.asset")).ToList();
                _instance.Games = AssetDatabase.LoadAssetAtPath<MinigamesScriptableObject>(miniGames[0]);

                var scoreCanvasPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.FindAssets("IngameScoreScreenPrefab").OrderBy(x => x).Select(x => AssetDatabase.GUIDToAssetPath(x)).Where(x => x.Contains("IngameScoreScreenPrefab.prefab")).ToList()[0]);
                var scp = Instantiate(scoreCanvasPrefab);
                _instance.ScoreCanvas = scp;
                _instance.ScoreScreen = scp.GetComponentInChildren<IngameScoreScreen>();
                _instance.ScoreCanvas.SetActive(true);
                _instance.ScoreScreen.Init();
            }
            return _instance;
        }
    }

    internal static void ShowPodiumBetweenGames()
    {
        SceneManager.LoadScene("InBetweenPodium");
    }

    public SignalRTest SignalR;
    public MinigamesScriptableObject Games;
    public int GameIndex = -1;

    public void Start()
    {
        _instance = this;
    }

    public void InitializeNewGame()
    {
        ScoreCanvas.SetActive(true);
        DontDestroyOnLoad(ScoreCanvas);
        ScoreScreen.Init();
        StartNextGame();
        ScoreCanvas.SetActive(false);
    }

    public void StartNextGame()
    {
        GameIndex++;
        if (GameIndex >= Games.Minigames.Count)
            GameIndex = 0;
        if (Games)
        {
            if (_instance.SignalR.Players.Count < Games.Minigames[GameIndex].MinPlayers || _instance.SignalR.Players.Count > Games.Minigames[GameIndex].MaxPlayers)
            {

                if (!Games.Minigames.Any(x => _instance.SignalR.Players.Count >= x.MinPlayers && _instance.SignalR.Players.Count <= x.MaxPlayers))
                {
                    throw new System.Exception("No matching games for playercount");
                }
                else
                {
                    StartNextGame();
                }
            }
            else
            {
                SceneManager.LoadScene(Games.Minigames[GameIndex].SceneName);
            }
        }
    }
}
