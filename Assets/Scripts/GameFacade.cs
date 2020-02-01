using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameFacade : MonoBehaviour
{
    //GameFacade控制所有Manager，游戏物体需要调用Manager中的内容时必须通过GameFacade这个中介
    //而Manager中需要被其他游戏物体调用的方法就转入Facade中（具体如Audio Region所示）
    //不同的Manager需要在Init中进行初始化操作，以及在Update、OnDestroy中注册生命周期函数

    private static GameFacade _instance;
    public static GameFacade Instance
    {
        get { return _instance; }
    }

    private AudioManager audioManager;
    private UIManager uiManager;

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

        audioManager.startSmoothSpeed = startSmoothSpeed;
        audioManager.stopSmoothSpeed = stopSmoothSpeed;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiManager.PushPanel(UIPanelType.Setting);
        }
    }

    private void Init()
    {
        audioManager = new AudioManager(this);
        uiManager = new UIManager(this);

        audioManager.OnInit();
        uiManager.OnInit();
    }

    private void UpdateManager()
    {
        audioManager.Update();
        uiManager.Update();
    }

    private void OnDestroy()
    {
        audioManager.OnDestroy();
        uiManager.OnDestroy();
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

    /// <summary>
    /// 设置音量
    /// </summary>
    /// <param name="bgVolume">背景音量</param>
    /// <param name="normalVolume">音效音量</param>
    public void SetVolume(float bgVolume, float normalVolume)
    {
        audioManager.SetVolume(bgVolume, normalVolume);
    }
    #endregion

    #region UI内容

    /// <summary>
    /// 显示提示信息
    /// </summary>
    /// <param name="msg">信息内容</param>
    public void ShowMessage(string msg)
    {
        uiManager.ShowMessage(msg);
    }

    #endregion
}