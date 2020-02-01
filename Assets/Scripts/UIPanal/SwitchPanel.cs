using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SwitchPanel : BasePanel
{
    private Image image;
    public override void OnEnter()
    {
        image = GetComponent<Image>();
        ShowAnim();
        base.OnEnter();
    }

    public override void OnResume()
    {
        ShowAnim();
        base.OnResume();
    }

    public override void OnPause()
    {
        ShowAnim();
        base.OnPause();
    }

    public void ShowAnim()
    {
        gameObject.SetActive(true);
        Color tempColor = image.color;
        tempColor.a = 0;
        image.color = tempColor;
        image.DOFade(1, 0.6f).OnComplete(() => image.DOFade(0, 0.6f).SetDelay(0.5f)
            .OnComplete(delegate()
            {
                gameObject.SetActive(false);
                uiManager.PopPanel();
            }));
    }
}
