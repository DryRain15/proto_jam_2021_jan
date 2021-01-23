using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
public class SoundController : MonoBehaviour
{
    public static SoundController self;

    public AudioSource bgmChannel;
    public AudioSource SFXChannel;

    public AudioClip rewind;
    public AudioClip bgm;

    private void Awake()
    {
        if (self != null)
        {
            Destroy(this.gameObject);
            return;
        }
        self = this;
        bgmChannel = gameObject.AddComponent<AudioSource>();
        SFXChannel = gameObject.AddComponent<AudioSource>();
        bgmChannel.loop = true;
        SFXChannel.loop = false;
        SFXChannel.clip = rewind;
        bgmChannel.clip = bgm;
        bgmChannel.Stop();
        SFXChannel.Stop();
        SFXChannel.volume = 0.6f;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        PlayBGM();
    }

    public void PlayBGM()
    {
        bgmChannel.Stop();
        bgmChannel.Play();
    }

    public void PlayRewindSFX()
    {
        SFXChannel.Play();
        StartCoroutine(ToneDown(1));
    }

    IEnumerator ToneDown(float duration)
    {
        bgmChannel.pitch = 0.5f;
        yield return new WaitForSeconds(duration);
        bgmChannel.pitch = 1.0f;
        SFXChannel.Stop();
    }
}
