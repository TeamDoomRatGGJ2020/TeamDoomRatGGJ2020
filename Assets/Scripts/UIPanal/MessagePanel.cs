using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : BasePanel
{
    private Text text;
    private float showTime = 2;
    private string message = null;

    public override void OnEnter()
    {
        base.OnEnter();
        text = GetComponent<Text>();
        uiManager.InjectMsgPanel(this);
        text.enabled = false;
    }

    void Update()
    {
        if (message!=null)
        {
            ShowMessage(message);
            message = null;
        }
    }

    public void ShowMessage(string msg)
    {
        transform.SetAsLastSibling();
        text.text = msg;
        if (!text.enabled)
        {
            text.enabled = true;
        }

        if (text.canvasRenderer.GetColor().a < 1)
        {
            float time = Mathf.Abs(text.canvasRenderer.GetColor().a - 1) / 5;
            text.CrossFadeAlpha(1, time, false);
            Invoke("Hide", showTime + time);
        }
        else
        {
            Invoke("Hide", showTime);
        }
    }

    private void Hide()
    {
        text.CrossFadeAlpha(0, 1, false);
    }
}
