using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour
{
    private bool IsHakkermanEdition;
    public GameObject ScoreCanvas;
    public IngameScoreScreen ScoreScreen;

    public PlayerToColorMappingScriptableObject PlayerColors;
    public ShipModuleScriptableObject AllModules;
    public GameObject NextModuleReward;

    public Color GetPlayerColor(int playerIndex)
    {
        return PlayerColors.Colors[playerIndex % PlayerColors.Colors.Count];
    }

    private static MinigameManager _instance;

    public static MinigameManager Instance
    {
        get
        {
#if UNITY_EDITOR
            if (_instance == null)
            {
                var editorOnlyHackForInstanceWorkStuff = new GameObject();

                //So that shit works in Editor as well
                _instance = editorOnlyHackForInstanceWorkStuff.AddComponent<MinigameManager>();
                _instance.SignalR = editorOnlyHackForInstanceWorkStuff.AddComponent<SignalRTest>();
                _instance.IsHakkermanEdition = true;

                var colors = AssetDatabase.FindAssets("PlayerColors").OrderBy(x => x).Select(x => AssetDatabase.GUIDToAssetPath(x)).Where(x => x.Contains("PlayerColors.asset")).ToList();
                _instance.PlayerColors = AssetDatabase.LoadAssetAtPath<PlayerToColorMappingScriptableObject>(colors[0]);

                var shipModels = AssetDatabase.FindAssets("ShipModules").OrderBy(x => x).Select(x => AssetDatabase.GUIDToAssetPath(x)).Where(x => x.Contains("ShipModules.asset")).ToList();
                _instance.AllModules = AssetDatabase.LoadAssetAtPath<ShipModuleScriptableObject>(shipModels[0]);

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
                    pc.PlayerColor = _instance.GetPlayerColor(i);
                    pc.PlayerMad = imgScrob.ImageSad;
                    pc.PlayerHappy = imgScrob.ImageWin;
                    DontDestroyOnLoad(keyboardPlayerControllerGA);
                    _instance.SignalR.Players.Add(i.ToString(), pc);
                }

                var miniGames = AssetDatabase.FindAssets("Minigames").OrderBy(x => x).Select(x => AssetDatabase.GUIDToAssetPath(x)).Where(x => x.Contains("Minigames.asset")).ToList();
                _instance.Games = AssetDatabase.LoadAssetAtPath<MinigamesScriptableObject>(miniGames[0]);

                var scoreCanvasPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.FindAssets("IngameScoreScreenPrefab").OrderBy(x => x).Select(x => AssetDatabase.GUIDToAssetPath(x)).Where(x => x.Contains("IngameScoreScreenPrefab.prefab")).ToList()[0]);
                var scp = Instantiate(scoreCanvasPrefab, editorOnlyHackForInstanceWorkStuff.transform);
                _instance.ScoreCanvas = scp;
                _instance.ScoreScreen = scp.GetComponentInChildren<IngameScoreScreen>();
                _instance.ScoreCanvas.SetActive(true);
                _instance.ScoreScreen.Init();
                _instance.NextModuleReward = _instance.AllModules.AllShipModules[Random.Range(0, _instance.AllModules.AllShipModules.Count)];

                DontDestroyOnLoad(_instance.gameObject);
            }
#endif
            return _instance;
        }
    }

    public void CompletelyRestartGameAndShit(string error)
    {
        MainMenu.ErrorToShow = error;
        SceneManager.LoadScene("MainMenu");
    }

    internal static void ShowRewardScene()
    {
        SceneManager.LoadScene("ClaimRewardScene");
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
        if (_instance != null && !IsHakkermanEdition)
        {
            //Kill it with fire
            _instance.SignalR.SignalR.Stop();
            GameObject.Destroy(_instance.SignalR.gameObject);
            GameObject.Destroy(_instance);
        }

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
        foreach (var p in SignalR.Players)
        {
            p.Value.ResetScore();
        }
        NextModuleReward = AllModules.AllShipModules[Random.Range(0, AllModules.AllShipModules.Count)];
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
