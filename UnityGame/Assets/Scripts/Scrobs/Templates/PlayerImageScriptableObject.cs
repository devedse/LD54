using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerImageScriptableObject", menuName = "ScriptableObject/Player Image")]
public class PlayerImageScriptableObject : ScriptableObject
{
    public Sprite ImageIdle;
    public Sprite ImageSad;
    public Sprite ImageWin;

    public AudioClip SoundHappy;
    public AudioClip SoundAngry;
    public AudioClip SoundNeutral;
}
