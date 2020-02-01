using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTrigger : MonoBehaviour
{
    //-1 刚刚开始游戏
    //0 看完回忆1
    //1 拿到木板
    //2 盖完桥
    //3 回忆2
    //4 拿到苹果
    //5 拿到木板和钉子
    //6 修完断桥
    private int MissionIndex = -1;

    //TODO
    //在离开场景，进入动画时，应禁止人物移动，尚未完成
    private GameFacade facade;
    private float timeCount;

    private Image bgImage = null;
    private Position enterPosition = Position.Null;
    private int index = -1;

    void Start()
    {
        facade = GameFacade.Instance;
        index = facade.GetPresentIndex();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Edge")
        {
            OnSwitchScene(collision);
        }
        else if (collision.tag == "Mission")
        {
            OnMission(collision);
        }
        else if (collision.tag == "Audio")
        {
            OnSwitchAudio(collision);
        }
    }

    private void OnSwitchScene(Collider collision)
    {
        bgImage = collision.GetComponentInParent<Image>();
        string name = collision.name;
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

    private void OnMission(Collider collision)
    {
        switch (index)
        {
            //回忆1
            case 0:
                //TODO
                break;
            //马路
            case 2:
                //TODO
                //string str = collision.name;
                //string[] strs = str.Split('_');
                //int triggerIndex = int.Parse(strs[1]);
                //if (triggerIndex == 0)
                //{
                    
                //}
                //else if (triggerIndex == 1)
                //{

                //}
                break;
            //有木板的小路
            //木板只有一块
            case 3:
                //TODO
                break;
            //开裂的小路
            //捡到木板之后搭上去
            case 4:
                //TODO
                break;
            //木匠小屋
            //在木匠身边有小对话气泡 按对话键开始对话 第一次开始对话开启任务（内容如大纲
            //没去断桥之前和木匠对话 主角不会询问木板和钉子 但是完成任务之后木匠还是会给木板和钉子 这个时候木板和钉子就放在一边
            //主角处于触发器范围内依旧可以拿起
            //去了断桥之后主角会询问木板和钉子 完成任务之后直接把木板和钉子顶在头上
            case 6:
                //TODO
                break;
            //断桥
            case 8:
                //TODO
                break;
            //回忆2
            case 10:
                //TODO
                break;
            //给木匠苹果的任务
            //如果在开启木匠任务之前使用方块砸地把苹果砸下来 苹果掉在地上 玩家按拾取键 苹果消失 播放吃苹果的音效
            //开启木匠任务之后 玩家按拾取键 直接把苹果顶在头上
            //完成任务之后同开启任务之前
            case 11:
                //TODO
                break;
        }
    }

    //触碰到Audio标签的Trigger 可直接切换音效
    private void OnSwitchAudio(Collider collision)
    {
        string name = collision.name;
        string[] strs = name.Split('_');
        switch (strs[0])
        {
            case "Grass":
                //TODO
                break;
            case "Footpath":
                //TODO
                break;
            case "Plank":
                //TODO
                break;
            case "Road":
                //TODO
                break;
        }
    }
}
