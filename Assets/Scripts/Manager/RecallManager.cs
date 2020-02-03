using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RecallManager : BaseManager
{
    public RecallManager(GameFacade facade) : base(facade) { }

    private float timeCount;

    public void OnRecall(int recallIndex,string hint = null)
    {
        if (facade.GetPresentGO().transform.Find("Recall_" + recallIndex) == null) 
        {
            return;
        }
        GameObject go = facade.GetPresentGO().transform.Find("Recall_" + recallIndex).gameObject;
        if (go == null)
        {
            return;
        }
        go.SetActive(true);
        facade.PlayBgSound(AudioManager.Sound_Recall);
        float time = 0;
        if (recallIndex == 1)
        {
            time = 7.5f;


            var cc = GameFacade.Instance.cc;
            //GameObject.Find("CM vcam1").GetComponent<Cinemachine.CinemachineVirtualCamera>().;
            cc.SetFocusOffset(-7);
        }
        else if (recallIndex == 2)
        {
            time = 8;

            var cc = GameFacade.Instance.cc;
            //GameObject.Find("CM vcam1").GetComponent<Cinemachine.CinemachineVirtualCamera>().;
            cc.SetFocusOffset(4);
        }
        DOTween.To(() => timeCount, a => timeCount = a, 1, time).OnComplete(delegate()
        {
            go.SetActive(false);
            facade.cc.ChangeMovable(true);
            if (hint != null)
            {
                facade.ShowMessage(hint);
            }

            facade.PlayBgSoundSmoothlySync(AudioManager.Sound_BGM);

            var cc = GameFacade.Instance.cc;
            //GameObject.Find("CM vcam1").GetComponent<Cinemachine.CinemachineVirtualCamera>().;
            cc.ResetFocus();
        });
        facade.cc.ChangeMovable(false);
    }
}
