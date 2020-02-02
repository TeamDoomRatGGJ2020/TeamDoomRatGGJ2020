using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : BaseManager
{
    public TalkManager(GameFacade facade) : base(facade) { }

    private GameObject talk = null;
    private Animator talkAnim = null;

    private void OnBeginTalk(string name)
    {
        GameObject go = facade.GetPresentGO().transform.Find(name).gameObject;
        if (go == null)
        {
            return;
        }

        talk = go;
        talkAnim = talk.GetComponent<Animator>();
        talkAnim.SetTrigger("Talk");
    }

    private void OnEndTalk()
    {
        if (talk == null)
        {
            return;
        }
        talkAnim.SetTrigger("EndTalk");
        talk = null;
        talkAnim = null;
    }

    public void OnCarpenterTalk()
    {
        OnBeginTalk("Carpenter");
    }
}
