using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionData
{
    public Vector3 LeftAirWallPosition { get; private set; }
    public Vector3 RightAirWallPosition { get; private set; }
    public Vector3 NearAirWallPosition { get; private set; }
    public Vector3 FarAirWallPosition { get; private set; }
    public Vector3 LeftEnterPosition { get; private set; }
    public Vector3 RightEnterPosition { get; private set; }
    public Vector3 UpEnterPosition { get; private set; }
    public Vector3 DownEnterPosition { get; private set; }

    public int Index { get; private set; }

    public PositionData(Vector3 leftAirWallPosition, Vector3 nearAirWallPosition,
        Vector3 farAirWallPosition, int index, Vector3 leftEnterPosition, Vector3 rightEnterPosition,
        Vector3 upEnterPosition, Vector3 downEnterPosition)
    {
        this.LeftAirWallPosition = leftAirWallPosition;
        this.RightAirWallPosition = new Vector3(-leftAirWallPosition.x, leftAirWallPosition.y, leftAirWallPosition.z);
        this.NearAirWallPosition = nearAirWallPosition;
        this.FarAirWallPosition = farAirWallPosition;
        this.Index = index;
        this.LeftEnterPosition = leftEnterPosition;
        this.RightEnterPosition = rightEnterPosition;
        this.UpEnterPosition = upEnterPosition;
        this.DownEnterPosition = downEnterPosition;
    }
}
