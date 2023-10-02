using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.PackageManager;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour
{
    //private bool IsHakkermanEdition;
    public GameObject ScoreCanvas;
    public List<IngameScoreScreen> ScoreScreens = new List<IngameScoreScreen>();

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
                //_instance.IsHakkermanEdition = true;

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
                    keyboardPlayerControllerGA.transform.SetParent(_instance.transform);
                    _instance.SignalR.Players.Add(i.ToString(), pc);

                    var randomModules = true;
                    if (randomModules)
                    {
                        for (int slot = 0; slot < 3; slot++)
                        {
                            var totalModules = System.Enum.GetValues(typeof(ShipModuleType)).Length - 1;

                            pc.SetModuleForSlot(slot, (ShipModuleType)((pc.PlayerIndex * 3 + slot) % totalModules) + 1);

                        }

                        //if (pc.PlayerIndex == 1)
                        //{
                        //    pc.SetModuleForSlot(1, ShipModuleType.Adelaar);
                        //}

                        //if (pc.PlayerIndex == 0)
                        //{
                        //    pc.SetModuleForSlot(0, ShipModuleType.Chainsaw);
                        //}
                    }
                }

                var miniGames = AssetDatabase.FindAssets("Minigames").OrderBy(x => x).Select(x => AssetDatabase.GUIDToAssetPath(x)).Where(x => x.Contains("Minigames.asset")).ToList();
                _instance.Games = AssetDatabase.LoadAssetAtPath<MinigamesScriptableObject>(miniGames[0]);

                var scoreCanvasPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.FindAssets("IngameScoreScreenPrefab").OrderBy(x => x).Select(x => AssetDatabase.GUIDToAssetPath(x)).Where(x => x.Contains("IngameScoreScreenPrefab.prefab")).ToList()[0]);
                var scp = Instantiate(scoreCanvasPrefab, editorOnlyHackForInstanceWorkStuff.transform);
                _instance.ScoreCanvas = scp;
                _instance.ScoreCanvas.SetActive(true);
                foreach (var scscr in scp.GetComponentsInChildren<IngameScoreScreen>())
                {
                    _instance.ScoreScreens.Add(scscr);
                    scscr.Init();
                }
                var currentScene = _instance.Games.AllMinigames.FirstOrDefault(x => x.SceneName == SceneManager.GetActiveScene().name);
                if (currentScene)
                {
                    _instance.ScoreCanvas.GetComponent<ScoreScreenShower>().Show(currentScene.ScoreScreenAlignment);
                }
                else
                {
                    _instance.ScoreCanvas.GetComponent<ScoreScreenShower>().HideAll();
                }
                _instance.NextModuleReward = _instance.AllModules.AllShipModules[Random.Range(0, _instance.AllModules.AllShipModules.Count)];

                DontDestroyOnLoad(_instance.gameObject);
            }
#endif
            return _instance;
        }
    }

    internal static void ShowStatsScene()
    {
        SceneManager.LoadScene("StatsScene");
    }

    private bool _desireCompleteAndUtterlyDestroyEverythingAndRestart = false;

    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        //if (_instance != null && !IsHakkermanEdition)
        //{
        //    //Kill it with fire
        //    GameObject.Destroy(_instance.gameObject);
        //}

        _instance = this;
    }

    public void CompletelyRestartGameAndShit(string error)
    {
        if (_desireCompleteAndUtterlyDestroyEverythingAndRestart == false)
        {
            MainMenu.ErrorToShow = error;
            _desireCompleteAndUtterlyDestroyEverythingAndRestart = true;
        }

        Debug.Log($"Received request to completely restart: {error}");
    }

    private void Update()
    {
        if (_desireCompleteAndUtterlyDestroyEverythingAndRestart)
        {
            Debug.Log("Update: Loading main scene again now");
            _instance.SignalR.SignalR?.Stop();
            GameObject.Destroy(_instance.gameObject);

            SceneManager.LoadScene("MainMenu");
        }
    }

    internal static void ShowRewardScene()
    {
        SceneManager.LoadScene("ClaimRewardScene");
        _instance.ScoreCanvas.GetComponent<ScoreScreenShower>().Show(ScoreScreenOptions.Top);
    }

    internal static void ShowPodiumBetweenGames()
    {
        SceneManager.LoadScene("InBetweenPodium");
        var mm = FindFirstObjectByType<MusicManager>();
        mm.PlayMusic(mm.TrophyMusic);
    }

    public SignalRTest SignalR;
    public MinigamesScriptableObject Games;
    public int GameIndex = -1;
    public bool GoToStatsNext;

    public void InitializeNewGame()
    {
        SignalR.LobbyHasStartedSoBlockNewPlayerJoins = true;

        ScoreCanvas.SetActive(true);
        foreach (var scscr in ScoreCanvas.GetComponentsInChildren<IngameScoreScreen>())
        {
            ScoreScreens.Add(scscr);
            scscr.Init();
        }
        StartNextGame();
        ScoreCanvas.SetActive(false);

    }
    internal void DoTiebreaker()
    {
        SceneManager.LoadScene(Games.TiebreakerMinigames[Random.Range(0, Games.TiebreakerMinigames.Count)].SceneName);
        ScoreCanvas.GetComponent<ScoreScreenShower>().Show(ScoreScreenOptions.Left);
    }

    public void StartNextGame()
    {
        var mm = FindFirstObjectByType<MusicManager>();
        mm.PlayMusic(mm.BattleMusic);
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
                ScoreCanvas.GetComponent<ScoreScreenShower>().Show(Games.Minigames[GameIndex].ScoreScreenAlignment);
            }
        }
    }
}
