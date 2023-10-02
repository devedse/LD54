using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "MinigameScriptableObject", menuName = "ScriptableObject/Minigame")]
public class MinigameScriptableObject : ScriptableObject
{
    public string SceneName;

    public int MinPlayers;
    public int MaxPlayers;

    public bool PlaySoundOnPlayerHappy;
    public bool PlaySoundOnPlayerSad;
    public ScoreScreenOptions ScoreScreenAlignment;
}
