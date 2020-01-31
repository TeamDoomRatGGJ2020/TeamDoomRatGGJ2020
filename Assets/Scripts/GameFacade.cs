using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFacade : MonoBehaviour
{
    //GameFacade控制所有Manager，游戏物体需要调用Manager中的内容时必须通过GameFacade这个中介
    //而Manager中需要被其他游戏物体调用的方法就转入Facade中（具体如Audio Region所示）

    private static GameFacade _instance;
    public static GameFacade Instance
    {
        get { return _instance; }
    }

    private AudioManager audioManager;

    public float stopSmoothSpeed = 1f;
    public float startSmoothSpeed = 1f;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);

            return;
        }
        _instance = this;
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        UpdateManager();

        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayBgSoundSmoothlySync(AudioManager.Sound_BirdCall);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            StopBgSoundSmoothlySync();
        }

        audioManager.startSmoothSpeed = startSmoothSpeed;
        audioManager.stopSmoothSpeed = stopSmoothSpeed;
    }

    private void Init()
    {
        audioManager = new AudioManager(this);

        audioManager.OnInit();
    }

    private void UpdateManager()
    {
        audioManager.Update();
    }

    private void OnDestory()
    {
        audioManager.OnDestory();
    }

    #region Audio内容

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="soundName">背景音乐路径</param>
    /// <param name="volume">音量</param>
    public void PlayBgSound(string soundName, float volume = 1f)
    {
        audioManager.PlayBgSound(soundName, volume);
    }

    /// <summary>
    /// 渐进播放背景音乐
    /// </summary>
    /// <param name="soundName">背景音乐路径</param>
    /// <param name="volume">音量</param>
    public void PlayBgSoundSmoothlySync(string soundName, float volume = 1f)
    {
        audioManager.PlayBgSoundSmoothlySync(soundName, volume);
    }

    /// <summary>
    /// 播放普通音效
    /// </summary>
    /// <param name="soundName">音效路径</param>
    /// <param name="audioSource">播放物体身上的AudioSource组件</param>
    /// <param name="volume">音量大小</param>
    public void PlayNormalSound(string soundName, AudioSource audioSource, float volume = 1f)
    {
        audioManager.PlayNormalSound(soundName, audioSource, volume);
    }

    /// <summary>
    /// 停止背景音乐播放
    /// </summary>
    public void StopBgSound()
    {
        audioManager.StopBgSound();
    }

    /// <summary>
    /// 停止普通音效播放
    /// </summary>
    /// <param name="audioSource">播放物体身上的AudioSource组件</param>
    public void StopNormalSound(AudioSource audioSource)
    {
        audioManager.StopNormalSound(audioSource);
    }

    /// <summary>
    /// 渐进停止背景音乐播放
    /// </summary>
    public void StopBgSoundSmoothlySync()
    {
        audioManager.StopBgSoundSmoothlySync();
    }
    #endregion
}
