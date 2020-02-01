using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    private Image bgImage;
    public Slider bgSlider;
    public Slider normalSlider;
    private Button closeButton;
    private Button quitButton;

    public override void OnEnter()
    {
        base.OnEnter();
        //bgSlider = transform.Find("BgVolume/Slider").GetComponent<Slider>();
        //normalSlider = transform.Find("NormalVolume/Slider").GetComponent<Slider>();
        closeButton = transform.Find("CloseButton").GetComponent<Button>();
        quitButton = transform.Find("QuitButton").GetComponent<Button>();

        closeButton.onClick.AddListener(OnCloseClick);
        quitButton.onClick.AddListener(OnQuitClick);
        EnterAnim();
    }

    void Update()
    {
        //if (bgSlider == null)
        //{
        //    bgSlider = transform.Find("BgVolume/Slider").GetComponent<Slider>();
        //    normalSlider = transform.Find("NormalVolume/Slider").GetComponent<Slider>();
        //}
        float bgVolume = bgSlider.value;
        float normalVolume = normalSlider.value;
        facade.SetVolume(bgVolume, normalVolume);
        print(bgSlider);
        print(bgSlider.value);
    }

    private void OnCloseClick()
    {
        ExitAnim();
    }

    private void OnQuitClick()
    {

    }

    private void EnterAnim()
    {
        gameObject.SetActive(true);
    }

    private void ExitAnim()
    {
        gameObject.SetActive(false);
        uiManager.PopPanel();
    }
}
