using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioManager
{
    static AudioSource music;
    static AudioSource SFX;

    static List<AudioClip> trainerBattleMusic = new List<AudioClip>();
    static List<AudioClip> wildBattleMusic = new List<AudioClip>();

    public static void Initialization(AudioSource musicSource, AudioSource sfxSource, List<AudioClip> trainerbattlemusic, List<AudioClip> wildbattlemusic)
    {
        music = musicSource;
        SFX = sfxSource;
        trainerBattleMusic = trainerbattlemusic;
        wildBattleMusic = wildbattlemusic;
    }

    public static void PlayMusic(AudioClip clip, float volume = 1, bool loop = true)
    {
        if (clip == music.clip)
            return;
        music.clip = clip;
        music.volume = volume;
        music.loop = loop;
        music.Play();
    }

    public static void PlaySingleSound(AudioClip clip, float volume = 1)
    {
        SFX.clip = clip;
        SFX.volume = volume;
        SFX.Play();
    }

    public static void PlayRandomBattleMusic(bool isTrainer)
    {
        if(isTrainer)
        {
            PlayMusic(trainerBattleMusic[Random.Range(0, trainerBattleMusic.Count - 1)]);
        }    
        else
        {
            PlayMusic(wildBattleMusic[Random.Range(0, wildBattleMusic.Count - 1)]);
        }
    }
}
