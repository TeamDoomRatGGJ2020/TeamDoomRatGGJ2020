using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : BaseManager
{
    public AudioManager(GameFacade facade) : base(facade) { }

    #region 音乐路径及名字
    private const string Sound_Prefix = "Sounds/";

    /// <summary>
    /// 吃苹果音效
    /// </summary>
    public const string Sound_EatApple = "吃苹果";
    /// <summary>
    /// 对话气泡音效
    /// </summary>
    public const string Sound_Dialogue = "对话气泡";
    /// <summary>
    /// 方块砸地音效
    /// </summary>
    public const string Sound_Smash = "方块砸地";
    /// <summary>
    /// 流水声
    /// </summary>
    public const string Sound_WaterRunning = "流水声";
    /// <summary>
    /// 鸟叫声
    /// </summary>
    public const string Sound_BirdCall = "鸟叫声";
    /// <summary>
    /// 汽车
    /// </summary>
    public const string Sound_Car = "汽车";
    /// <summary>
    /// 走路（草地）
    /// </summary>
    public const string Sound_Walk_Grass = "走路（草地）";
    /// <summary>
    /// 走路（小路）
    /// </summary>
    public const string Sound_Walk_Foodpath = "走路（小路）";
    /// <summary>
    /// 走路（木板）
    /// </summary>
    public const string Sound_Walk_Plank = "走路（木板）";
    /// <summary>
    /// 走路（马路）
    /// </summary>
    public const string Sound_Walk_Road = "走路（马路）";
    #endregion

    private AudioSource bgAudioSource;
    private AudioSource normalAudioSource;

    private bool needStopBg = false;
    private bool needPlayBg = false;
    private string soundName = null;
    private float maxVolume = 0;

    public float stopSmoothSpeed = 1f;
    public float startSmoothSpeed = 1f;

    public override void OnInit()
    {
        GameObject audioSourceGO = new GameObject("AudioSource(GameObject)");
        bgAudioSource = audioSourceGO.AddComponent<AudioSource>();
        normalAudioSource = audioSourceGO.AddComponent<AudioSource>();
    }

    public override void Update()
    {
        if (needStopBg)
        {
            StopBgSoundSmoothly();
        }

        if (needPlayBg)
        {
            PlayBgSoundSmooth();
        }
    }

    public void PlayBgSound(string soundName, float volume = 1f)
    {
        PlaySound(bgAudioSource, LoadSound(soundName), volume, true);
    }

    public void PlayBgSoundSmoothlySync(string soundName, float volume = 1f)
    {
        needPlayBg = true;
        bgAudioSource.volume = 0;
        bgAudioSource.loop = true;
        this.soundName = soundName;
        this.maxVolume = volume;
    }

    private void PlayBgSoundSmooth()
    {
        bgAudioSource.volume = Mathf.Lerp(bgAudioSource.volume, maxVolume, startSmoothSpeed * Time.deltaTime);
        bgAudioSource.clip = LoadSound(soundName);
        if (bgAudioSource.isPlaying == false)
        {
            bgAudioSource.Play();
        }

        if (Mathf.Abs(bgAudioSource.volume - maxVolume) <= 0.1f)
        {
            bgAudioSource.volume = maxVolume;
            soundName = null;
            maxVolume = 0;
            needPlayBg = false;
        }
    }

    public void PlayNormalSound(string soundName, AudioSource audioSource, float volume = 1f)
    {
        PlaySound(audioSource, LoadSound(soundName), volume);
    }

    public void StopBgSound()
    {
        if (bgAudioSource.isPlaying == false)
        {
            return;
        }
        bgAudioSource.Stop();
        needStopBg = false;
    }
    
    public void StopNormalSound(AudioSource audioSource)
    {
        if (audioSource.isPlaying == false)
        {
            return;
        }
        audioSource.Stop();
    }

    public void StopBgSoundSmoothlySync()
    {
        needStopBg = true;
    }

    private void StopBgSoundSmoothly()
    {
        if (bgAudioSource.isPlaying == false)
        {
            return;
        }

        bgAudioSource.volume = Mathf.Lerp(bgAudioSource.volume, 0f, stopSmoothSpeed * Time.deltaTime);

        if (bgAudioSource.volume <= 0.1f) 
        {
            StopBgSound();
        }
    }

    private void PlaySound(AudioSource audioSource, AudioClip clip, float volume, bool needLoop = false)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = needLoop;
        audioSource.Play();
    }

    private AudioClip LoadSound(string soundName)
    {
        return Resources.Load<AudioClip>(Sound_Prefix + soundName);
    }
}