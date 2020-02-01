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
    /// 背景音乐
    /// </summary>
    public const string Sound_BGM = "BGM";
    /// <summary>
    /// 按钮点击音效
    /// </summary>
    public const string Sound_ButtonClick = "ButtonClick";
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
    /// 汽车
    /// </summary>
    public const string Sound_Car = "汽车";
    /// <summary>
    /// 流水声
    /// </summary>
    public const string Sound_WaterRunning = "流水声";
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
    /// <summary>
    /// 鸟叫声
    /// </summary>
    public const string Sound_BirdCall = "鸟叫声";
    #endregion

    private AudioSource bgAudioSource;
    private AudioSource normalAudioSource;

    private bool needStopBg = false;
    private bool needPlayBg = false;
    private string soundName = null;

    private float bgVolume = 1f;
    private float normalVolume = 1f;

    public float stopSmoothSpeed = 1f;
    public float startSmoothSpeed = 1f;
    public override void OnInit()
    {
        GameObject audioSourceGO = new GameObject("AudioSource(GameObject)");
        bgAudioSource = audioSourceGO.AddComponent<AudioSource>();
        normalAudioSource = audioSourceGO.AddComponent<AudioSource>();
        PlayBgSound(Sound_BGM);
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

        bgAudioSource.volume = bgVolume;
        normalAudioSource.volume = normalVolume;
    }

    public void PlayBgSound(string soundName)
    {
        PlaySound(bgAudioSource, LoadSound(soundName), bgVolume, true);
    }

    public void PlayBgSoundSmoothlySync(string soundName)
    {
        needPlayBg = true;
        bgAudioSource.volume = 0;
        bgAudioSource.loop = true;
        this.soundName = soundName;
    }

    private void PlayBgSoundSmooth()
    {
        bgAudioSource.volume =
            Mathf.Lerp(bgAudioSource.volume, bgVolume, startSmoothSpeed * Time.deltaTime);
        bgAudioSource.clip = LoadSound(soundName);
        if (bgAudioSource.isPlaying == false)
        {
            bgAudioSource.Play();
        }

        if (Mathf.Abs(bgAudioSource.volume - bgVolume) <= 0.1f)
        {
            bgAudioSource.volume = bgVolume;
            soundName = null;
            needPlayBg = false;
        }
    }

    public void PlayNormalSound(string soundName, AudioSource audioSource)
    {
        if (audioSource == null)
        {
            PlaySound(normalAudioSource, LoadSound(soundName), normalVolume);
        }
        else
        {
            PlaySound(audioSource, LoadSound(soundName), normalVolume);
        }
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

    public void SetVolume(float bgVolume, float normalVolume)
    {
        this.bgVolume = bgVolume;
        this.normalVolume = normalVolume;
    }

    private AudioClip LoadSound(string soundName)
    {
        return Resources.Load<AudioClip>(Sound_Prefix + soundName);
    }
}