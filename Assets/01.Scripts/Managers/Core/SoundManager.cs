﻿using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    public void Init()
    {
        GameObject root = new GameObject { name = "@Sound" };
        Object.DontDestroyOnLoad(root);

        string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));
        for (int i = 0; i < soundNames.Length - 1; i++)
        {
            GameObject go = new GameObject { name = soundNames[i] };
            _audioSources[i] = go.AddComponent<AudioSource>();
            go.transform.parent = root.transform;
        }
        _audioSources[(int)Define.Sound.Bgm].loop = true;
    }
    public void Clear()
    {
        // foreach (AudioSource audioSource in _audioSources)
        // {
        //     audioSource.clip = null;
        //     audioSource.Stop();
        // }
        // _audioClips.Clear();
    }
    public void Play(string path, Define.Sound type = Define.Sound.Effect, float volume = 1.0f)
    {
        if (!Managers.Data.UseSound) return;

        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, volume);
    }
    private void Play(AudioClip audioClip, Define.Sound type = Define.Sound.Effect, float volume = 1.0f)
    {
        if (!Managers.Data.UseSound) return;

        if (audioClip == null)
            return;

        if (type == Define.Sound.Bgm)
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.volume = volume;
            audioSource.clip = audioClip;
            audioSource.Play();

            oriBgmVol = volume;
        }
        else
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];
            audioSource.volume = volume;
            audioSource.PlayOneShot(audioClip);
        }
    }

    float oriBgmVol;
    public void BgmOnOff(bool isOn)
    {
        _audioSources[(int)Define.Sound.Bgm].volume = isOn ? oriBgmVol : 0;
    }

    AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.Effect)
    {
        AudioClip audioClip = null;

        if (type == Define.Sound.Bgm)
        {
            audioClip = Managers.Resource.Load<AudioClip>(path);
        }
        else
        {
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Managers.Resource.Load<AudioClip>(path);
                _audioClips.TryAdd(path, audioClip);
            }
        }

        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {path}");

        return audioClip;
    }
}
