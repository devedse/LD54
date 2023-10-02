using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource SoundSource;

    public SoundsScriptableObject Sounds;
    // Start is called before the first frame update
    void Start()
    {
        SoundSource.volume = S.IngameVolume;
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }

    public static void PlaySound(AudioClip clip, float volume = 1)
    {
        if (clip)
            Instance.SoundSource.PlayOneShot(clip, volume);
    }
}
