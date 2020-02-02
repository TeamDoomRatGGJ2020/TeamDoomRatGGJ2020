using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    private SceneManager sceneManager;
    private PositionManager positionManager;
    private RecallManager recallManager;
    private TalkManager talkManager;
    private HeadManager headManager;

    public CharacterController cc;

    private float timeCount;

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
        cc = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();

        audioManager = new AudioManager(this);
        uiManager = new UIManager(this);
        sceneManager = new SceneManager(this);
        positionManager = new PositionManager(this);
        recallManager = new RecallManager(this);
        talkManager = new TalkManager(this);
        headManager = new HeadManager(this);

        audioManager.OnInit();
        uiManager.OnInit();
        sceneManager.OnInit();
        positionManager.OnInit();
        recallManager.OnInit();
        talkManager.OnInit();
        headManager.OnInit();
    }

    private void UpdateManager()
    {
        audioManager.Update();
        uiManager.Update();
        sceneManager.Update();
        positionManager.Update();
        recallManager.Update();
        talkManager.Update();
        headManager.Update();
    }

    private void OnDestroy()
    {
        audioManager.OnDestroy();
        uiManager.OnDestroy();
        sceneManager.OnDestroy();
        positionManager.OnDestroy();
        recallManager.OnDestroy();
        talkManager.OnDestroy();
        headManager.OnDestroy();
    }

    #region Audio内容

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="soundName">背景音乐路径</param>
    /// <param name="volume">音量</param>
    public void PlayBgSound(string soundName)
    {
        audioManager.PlayBgSound(soundName);
    }

    /// <summary>
    /// 渐进播放背景音乐
    /// </summary>
    /// <param name="soundName">背景音乐路径</param>
    /// <param name="volume">音量</param>
    public void PlayBgSoundSmoothlySync(string soundName)
    {
        audioManager.PlayBgSoundSmoothlySync(soundName);
    }

    /// <summary>
    /// 播放普通音效
    /// </summary>
    /// <param name="soundName">音效路径</param>
    /// <param name="audioSource">播放物体身上的AudioSource组件</param>
    /// <param name="volume">音量大小</param>
    public void PlayNormalSound(string soundName, AudioSource audioSource)
    {
        audioManager.PlayNormalSound(soundName, audioSource);
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

    /// <summary>
    /// 显示切换动画
    /// </summary>
    public void ShowSwitch()
    {
        uiManager.ShowSwitch();
    }
    #endregion

    #region Scene内容

    /// <summary>
    /// 根据编码加载场景
    /// </summary>
    /// <param name="index">场景编码</param>
    public void SwitchScene(int index,Position enterPosition)
    {
        uiManager.ShowSwitch();
        DOTween.To(() => timeCount, a => timeCount = a, 1, 0.7f).OnComplete(delegate()
            {
                sceneManager.SwitchScene(index);
                SetPosition(index, enterPosition);
            });
    }

    /// <summary>
    /// 取得当前场景编码
    /// </summary>
    /// <returns>当前场景编码</returns>
    public int GetPresentIndex()
    {
        return sceneManager.GetPresentIndex();
    }

    public GameObject GetPresentGO()
    {
        return sceneManager.GetPresentGO();
    }

    #endregion

    #region Position内容

    /// <summary>
    /// 设置玩家的位置
    /// </summary>
    /// <param name="index">场景编号</param>
    /// <param name="enterPosition">触碰场景边缘的位置</param>
    public void SetPosition(int index, Position enterPosition)
    {
        positionManager.SetPosition(index, enterPosition);
    }

    public void SetRotation(Position position)
    {
        positionManager.SetRotation(position);
    }
    #endregion

    #region Recall内容

    public void OnRecall(int recallIndex, string hint = null)
    {
        recallManager.OnRecall(recallIndex, hint);
    }

    #endregion

    #region Talk内容

    public void OnCarpenterTalk()
    {
        talkManager.OnCarpenterTalk();
    }

    #endregion

    #region CC内容

    public void ChangeMovable(bool canMove)
    {
        cc.ChangeMovable(canMove);
    }

    #endregion

    #region Head内容

    public void PickUpPlank()
    {
        headManager.PickUpPlank();
    }

    public void PickUpApple()
    {
        headManager.PickUpApple();
    }

    public void Throw()
    {
        headManager.Throw();
    }
    #endregion
}