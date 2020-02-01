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
            int index = facade.GetPresentIndex();
            if (index != 10 && index != 11)
            {
                switch (name)
                {
                    case "RightEdge":
                        if (index == 8)
                        {
                            //TODO
                            return;
                        }
                        index += 1;
                        enterPosition = Position.Right;
                        facade.SwitchScene(index, enterPosition);
                        break;
                    case "LeftEdge":
                        index -= 1;
                        enterPosition = Position.Left;
                        facade.SwitchScene(index, enterPosition);
                        break;
                    case "UpEdge":
                        if (index == 5)
                        {
                            index = 10;
                            facade.SwitchScene(index, enterPosition);
                        }
                        else if (index == 7)
                        {
                            index = 11;
                            facade.SwitchScene(index, enterPosition);
                        }
                        enterPosition = Position.Up;
                        break;
                }
            }
            else
            {
                switch (name)
                {
                    case "RightEdge":
                        break;
                    case "LeftEdge":
                        if (index == 10)
                        {
                            index = 5;
                        }
                        else if (index == 11)
                        {
                            index = 7;
                        }
                        enterPosition = Position.Down;
                        facade.SwitchScene(index, enterPosition);
                        break;
                }
            }
        }
    }
}
