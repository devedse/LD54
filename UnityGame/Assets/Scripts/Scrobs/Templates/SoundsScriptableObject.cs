using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundsScriptableObject", menuName = "ScriptableObject/Sounds")]
public class SoundsScriptableObject : ScriptableObject
{
    public AudioClip ButtonClick;
    public AudioClip JoinedLobby;
    public AudioClip NextGameLoaded;
    public AudioClip FinishCountdown;
    public AudioClip FinishGame;
    public AudioClip ShowClaimReward;
    public AudioClip ShowStats;
    public AudioClip CheesePlayerFinishedStack;
    public AudioClip CheesePlayerRemovedBlock;
    public AudioClip FlappyFlap;
    public AudioClip FWNSpray;
    public AudioClip FWNWhirlwind;
    public AudioClip FWNDeath;
    public AudioClip AwardChooseShip;
    public AudioClip AwardChooseSlot;
    public AudioClip AwardChosenAndDone;
    public AudioClip ShowTrophiesScene;
    public AudioClip CountdownNumberChanged;
    public AudioClip DiggerinoUncoveredLava;
    public AudioClip DiggerinoUncoveredGrass;

    public AudioClip FinishTutorial { get; internal set; }
}
