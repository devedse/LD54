using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource MusicSource;

    public AudioClip MenuMusic;
    public AudioClip BattleMusic;
    public AudioClip TrophyMusic;

    // Start is called before the first frame update
    void Start()
    {
        AudioListener.volume = S.GlobalVolume;
        MusicSource.volume = S.MusicVolume;
        var allMusicManagers = FindObjectsByType<MusicManager>(FindObjectsSortMode.None);
        if (allMusicManagers.Length > 1)
        {
            foreach (var mm in allMusicManagers)
            {
                if (mm != this)
                {
                    mm.PlayMusic(MenuMusic);
                }
            }
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            PlayMusic(MenuMusic);
        }
    }

    public void PlayMusic(AudioClip music)
    {
        StopMusic();
        MusicSource.clip = music;
        MusicSource.Play();
        foreach (var bonker in FindObjectsByType<BeatBonker>(FindObjectsSortMode.InstanceID))
        {
            bonker.StartBonking();
        }
    }

    public void StopMusic()
    {
        MusicSource.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
