using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTrigger : MonoBehaviour
{
    //TODO
    //在离开场景，进入动画时，应禁止人物移动，尚未完成
    private GameFacade facade;
    private float timeCount;

    private Image bgImage = null;
    private Position enterPosition = Position.Null;

    void Start()
    {
        facade = GameFacade.Instance;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Edge")
        {
            bgImage = collision.GetComponentInParent<Image>();
            string name = collision.name;
            int index = -1;
            switch (name)
            {
                case "RightEdge":
                    index = facade.GetPresentIndex() + 1;
                    enterPosition = Position.Right;
                    facade.SwitchScene(index, enterPosition);
                    break;
                case "LeftEdge":
                    index = facade.GetPresentIndex() - 1;
                    enterPosition = Position.Left;
                    facade.SwitchScene(index, enterPosition);
                    break;
                case "UpEdge":
                    break;
            }
        }
    }
}
