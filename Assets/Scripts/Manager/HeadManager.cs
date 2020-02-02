using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadManager : BaseManager
{
    public HeadManager(GameFacade facade) : base(facade) { }

    private SpriteRenderer sr;

    private Sprite plankSprite = null;
    private Sprite appleSprite = null;

    private bool hasThing = false;

    public override void OnInit()
    {
        base.OnInit();
        sr = facade.cc.transform.Find("Head").GetComponent<SpriteRenderer>();

        plankSprite = Resources.Load<Sprite>("Elements/木板");
        appleSprite = Resources.Load<Sprite>("Elements/苹果");
    }

    public void PickUpPlank()
    {
        if (hasThing)
        {
            return;
        }
        sr.sprite = plankSprite;
        hasThing = true;
    }

    public void Throw()
    {
        sr.sprite = null;
        hasThing = false;
    }

    public void PickUpApple()
    {
        if (hasThing)
        {
            return;
        }
        sr.sprite = appleSprite;
        hasThing = true;
    }

    public bool HasThing()
    {
        return hasThing;
    }
}
