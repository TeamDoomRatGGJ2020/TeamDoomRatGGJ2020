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
        GameObject go = facade.GetPresentGO().transform.Find("Recall_" + recallIndex).gameObject;
        if (go == null)
        {
            return;
        }
        else
        {
            go.SetActive(true);
        }
        DOTween.To(() => timeCount, a => timeCount = a, 1, 7.5f).OnComplete(delegate()
        {
            go.SetActive(false);
            facade.cc.ChangeMovable(true);
            if (hint != null)
            {
                facade.ShowMessage(hint);
            }
        });
        facade.cc.ChangeMovable(false);
    }
}
