using Hapiga.Core.Runtime.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public Sound[] backgroundSounds;
    [Space]
    public Sound[] sounds;
    private int currentBGMusic = 0;
    public override void Init()
    {
        InitialSound();
    }
    private void InitialSound()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        foreach (Sound s in backgroundSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }


    public void PlayBGMusic()
    {
        if (!GameManager.Instance.MusicOn)
            return;
        StartCoroutine(PlayBgMusic());
    }

    private IEnumerator PlayBgMusic()
    {
        backgroundSounds[currentBGMusic].source.Play();
        yield return null;
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }
    public void PlayInUpdate(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        if(!s.source.isPlaying)
        {
            s.source.Play();
        }
        
    }
    public void CustomVolum(string name, float newvolume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;

        s.source.volume = newvolume;
    }
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;

        if (s.source.isPlaying)
            s.source.Stop();
    }

    public void MuteMusic()
    {
        foreach (Sound s in backgroundSounds)
        {
            if (s.source.isPlaying)
                s.source.Stop();
        }
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.25f;

    [Range(0.1f, 3f)]
    public float pitch = 1;
    public bool loop;

    [HideInInspector]
    public AudioSource source;
}