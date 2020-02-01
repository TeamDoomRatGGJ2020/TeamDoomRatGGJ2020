﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;

public enum Position
{
    Left,
    Right,
    Up,
    Down,
    Null
}

public class PositionManager : BaseManager
{
    //TODO
    //暂时尚未做上与下边界的切换场景
    //建议：玩家移动的位置可以进行一些修改
    //      我测试了一下，发现之前的位置会冲出空气墙（没大明白Near和Far的空气墙是咋运作的hh，尝试了下初始位置的z发现0.4貌似不会出去，但不确定
    public PositionManager(GameFacade facade) : base(facade) { }

    private Transform playerTransform;
    private Transform leftAirWallTransform;
    private Transform rightAirWallTransform;

    private int index;

    private Vector3 leftScale = Vector3.zero;
    private Vector3 rightScale = Vector3.zero;

    private Vector3 position0_R = new Vector3(12, 0, 0.4f);
    private Vector3 position1_R = new Vector3(14, 0, 0.4f);
    private Vector3 position1_L = new Vector3(-14, 0, 0.4f);
    private Vector3 position1_U = new Vector3(0, 0, -1);
    private Vector3 position2_L = new Vector3(-17, 0, 0.4f);

    private Vector3 leftAirWallPosition_0 = new Vector3(-16.2f, 0, 0);
    private Vector3 rightAirrWallPositon_0 = new Vector3(16.2f, 0, 0);
    private Vector3 leftAirWallPosition_1 = new Vector3(-17.8f, 0, 0);
    private Vector3 rightAirrWallPositon_1 = new Vector3(17.8f, 0, 0);
    private Vector3 leftAirWallPosition_2 = new Vector3(-21, 0, 0);
    private Vector3 rightAirrWallPositon_2 = new Vector3(21, 0, 0);


    public override void OnInit()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        leftAirWallTransform = GameObject.Find("LeftAirWall").GetComponent<Transform>();
        rightAirWallTransform = GameObject.Find("RightAirWall").GetComponent<Transform>();
        leftScale = playerTransform.localScale;
        if (leftScale.x >= 0)
        {
            leftScale.x = 0 - leftScale.x;
        }

        rightScale = new Vector3(-leftScale.x, leftScale.y, leftScale.z);

        index = facade.GetPresentIndex();
        if (index == 0)
        {
            leftAirWallTransform.localPosition = leftAirWallPosition_0;
            rightAirWallTransform.localPosition = rightAirrWallPositon_0;
        }
        else if (index == 10 && index == 11)
        {
            leftAirWallTransform.localPosition = leftAirWallPosition_2;
            rightAirWallTransform.localPosition = rightAirrWallPositon_2;
        }
        else
        {
            leftAirWallTransform.localPosition = leftAirWallPosition_1;
            rightAirWallTransform.localPosition = rightAirrWallPositon_1;
        }
    }

    public void SetPosition(int index, Position enterPosition)
    {
        switch (index)
        {
            case 0:
                playerTransform.localPosition = position0_R;
                playerTransform.localScale = leftScale;
                leftAirWallTransform.localPosition = leftAirWallPosition_0;
                rightAirWallTransform.localPosition = rightAirrWallPositon_0;
                break;
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
                switch (enterPosition)
                {
                    case Position.Right:
                    case Position.Up:
                        playerTransform.localPosition = position1_L;
                        playerTransform.localScale = rightScale;
                        break;
                    case Position.Left:
                        playerTransform.localPosition = position1_R;
                        playerTransform.localScale = leftScale;
                        break;
                    case Position.Down:
                        playerTransform.localPosition = position1_U;
                        playerTransform.localScale = leftScale;
                        break;
                }
                leftAirWallTransform.localPosition = leftAirWallPosition_1;
                rightAirWallTransform.localPosition = rightAirrWallPositon_1;
                break;
            case 10:
            case 11:
                playerTransform.localPosition = position2_L;
                playerTransform.localScale = rightScale;
                leftAirWallTransform.localPosition = leftAirWallPosition_2;
                rightAirWallTransform.localPosition = rightAirrWallPositon_2;
                break;
        }
    }
}
