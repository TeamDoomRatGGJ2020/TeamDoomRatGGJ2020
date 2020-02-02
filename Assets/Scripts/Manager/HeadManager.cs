using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadManager : BaseManager
{
    public HeadManager(GameFacade facade) : base(facade) { }

    private SpriteRenderer sr;

    private Sprite plankSprite = null;
    private Sprite appleSprite = null;

    public override void OnInit()
    {
        base.OnInit();
        sr = facade.cc.transform.Find("Head").GetComponent<SpriteRenderer>();

        plankSprite = Resources.Load<Sprite>("Elements/木板");
        //TODO Apple
    }

    public void PickUpPlank()
    {
        sr.sprite = plankSprite;
    }

    public void Throw()
    {
        sr.sprite = null;
    }

    public void PickUpApple()
    {
        sr.sprite = appleSprite;
    }
}
