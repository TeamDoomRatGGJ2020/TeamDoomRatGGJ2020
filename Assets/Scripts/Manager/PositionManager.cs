using System.Collections;
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
    private Transform farAirWallTransform;
    private Transform nearAirWallTransform;

    private Dictionary<int, PositionData> positionDataDict = new Dictionary<int, PositionData>();

    private int index;

    private Vector3 leftScale = Vector3.zero;
    private Vector3 rightScale = Vector3.zero;

    public override void OnInit()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        leftAirWallTransform = GameObject.Find("LeftAirWall").GetComponent<Transform>();
        rightAirWallTransform = GameObject.Find("RightAirWall").GetComponent<Transform>();
        farAirWallTransform = GameObject.Find("FarAirWall").GetComponent<Transform>();
        nearAirWallTransform = GameObject.Find("NearAirWall").GetComponent<Transform>();
        SetPositionData();

        leftScale = playerTransform.localScale;
        if (leftScale.x >= 0)
        {
            leftScale.x = 0 - leftScale.x;
        }
        rightScale = new Vector3(-leftScale.x, leftScale.y, leftScale.z);

        index = facade.GetPresentIndex();
        PositionData pd = null;
        positionDataDict.TryGetValue(index, out pd);

        if (pd != null)
        {
            SetAirWallPosition(pd);
        }
    }

    public void SetPosition(int index, Position enterPosition)
    {
        PositionData pd = null;
        positionDataDict.TryGetValue(index, out pd);
        switch (index)
        {
            case 0:
                playerTransform.localPosition = pd.RightEnterPosition;
                playerTransform.localScale = leftScale;
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
                        playerTransform.localPosition = pd.LeftEnterPosition;
                        playerTransform.localScale = rightScale;
                        break;
                    case Position.Left:
                        playerTransform.localPosition = pd.RightEnterPosition;
                        playerTransform.localScale = leftScale;
                        break;
                    case Position.Down:
                        //TODO
                        playerTransform.localPosition = pd.RightEnterPosition;
                        playerTransform.localScale = leftScale;
                        break;
                }
                break;
            case 10:
            case 11:
                playerTransform.localPosition = pd.LeftEnterPosition;
                playerTransform.localScale = rightScale;
                break;
        }
        SetAirWallPosition(pd);
    }

    private void SetAirWallPosition(PositionData pd)
    {
        leftAirWallTransform.localPosition = pd.LeftAirWallPosition;
        rightAirWallTransform.localPosition = pd.RightAirWallPosition;
        nearAirWallTransform.localPosition = pd.NearAirWallPosition;
        farAirWallTransform.localPosition = pd.FarAirWallPosition;
    }

    private void SetPositionData()
    {
        positionDataDict.Add(0,
            new PositionData(new Vector3(-16.2f, 0, 0), new Vector3(0, 0, -7.5f),
                new Vector3(0, 0, 3), 0, Vector3.zero,
                new Vector3(12, 0, -1.4f), Vector3.zero, Vector3.zero));

        positionDataDict.Add(1,
            new PositionData(new Vector3(-17.8f, 0, 0), new Vector3(0, 0, -7.5f),
                new Vector3(0, 0, 2), 1, new Vector3(-14, 0, -1.5f),
                new Vector3(14.5f, 0, 1.1f), Vector3.zero, Vector3.zero));

        positionDataDict.Add(2,
            new PositionData(new Vector3(-17.8f, 0, 0), new Vector3(0, 0, -7.5f),
                new Vector3(0, 0, 4.5f), 2, new Vector3(-10, 0, 4.2f),
                new Vector3(14, 0, -6), Vector3.zero, Vector3.zero));

        positionDataDict.Add(3,
            new PositionData(new Vector3(-17.8f, 0, 0), new Vector3(0, 0, -7.5f),
                new Vector3(0, 0, 0.8f), 3, new Vector3(-15.5f, 0, -2.5f),
                new Vector3(15, 0, -1), Vector3.zero, Vector3.zero));

        positionDataDict.Add(4,
            new PositionData(new Vector3(-17.8f, 0, 0), new Vector3(0, 0, -7.5f),
                new Vector3(0, 0, 0.8f), 4, new Vector3(-15.3f, 0, -3.1f),
                new Vector3(15.5f, 0, -0.3f), Vector3.zero, Vector3.zero));

        positionDataDict.Add(5,
            new PositionData(new Vector3(-17.8f, 0, 0), new Vector3(0, 0, -7.5f),
                new Vector3(0, 0, 0.8f), 5, new Vector3(-15.3f, 0, -3.1f),
                new Vector3(15.5f, 0, -0.3f), Vector3.zero, Vector3.zero));

        positionDataDict.Add(6,
            new PositionData(new Vector3(-17.8f, 0, 0), new Vector3(0, 0, -7.5f),
                new Vector3(0, 0, 0.8f), 6, new Vector3(-15.3f, 0, -3.1f),
                new Vector3(15.5f, 0, -0.3f), Vector3.zero, Vector3.zero));

        positionDataDict.Add(7,
            new PositionData(new Vector3(-17.8f, 0, 0), new Vector3(0, 0, -7.5f),
                new Vector3(0, 0, 0.8f), 7, new Vector3(-15.3f, 0, -3.1f),
                new Vector3(15.5f, 0, -0.3f), Vector3.zero, Vector3.zero));

        positionDataDict.Add(8,
            new PositionData(new Vector3(-17.8f, 0, 0), new Vector3(0, 0, -7.5f),
                new Vector3(0, 0, 0.8f), 8, new Vector3(-15.3f, 0, -3.1f),
                new Vector3(15.5f, 0, -0.3f), Vector3.zero, Vector3.zero));

        positionDataDict.Add(10,
            new PositionData(new Vector3(-21f, 0, 0), new Vector3(0, 0, -7.5f),
                new Vector3(0, 0, 0.8f), 10, new Vector3(-15.8f, 0, 0.32f),
                Vector3.zero,Vector3.zero, Vector3.zero));

        positionDataDict.Add(11,
            new PositionData(new Vector3(-21f, 0, 0), new Vector3(0, 0, -7.5f),
                new Vector3(0, 0, 0.8f), 11, new Vector3(-15.3f, 0, 0.32f),
                Vector3.zero, Vector3.zero, Vector3.zero));
    }
}
