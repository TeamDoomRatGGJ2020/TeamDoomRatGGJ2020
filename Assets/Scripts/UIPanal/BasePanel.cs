﻿using UnityEngine;
using System.Collections;

public class BasePanel : MonoBehaviour
{
    protected UIManager uiManager;
    public UIManager UIManager
    {
        set { uiManager = value; }
    }

    protected GameFacade facade;
    public GameFacade Facade
    {
        set { facade = value; }
    }

    protected void PlayClickSound()
    {
        facade.PlayNormalSound(AudioManager.Sound_ButtonClick, null);
    }
    /// <summary>
    /// 界面被显示出来
    /// </summary>
    public virtual void OnEnter()
    {

    }

    /// <summary>
    /// 界面暂停
    /// </summary>
    public virtual void OnPause()
    {

    }

    /// <summary>
    /// 界面继续
    /// </summary>
    public virtual void OnResume()
    {

    }

    /// <summary>
    /// 界面不显示,退出这个界面，界面被关系
    /// </summary>
    public virtual void OnExit()
    {

    }
}