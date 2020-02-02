using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CrossRoadStateEnum
{
    Refuse,
    WaitForChangeShape,
    WaitNoCar,
    Crossing,
    Finished
}

public class RoadController : MonoBehaviour
{
    // public MemoryOne memory;
    // public float CarScale = 1f;
    public float WaitNoCarTime = 3f;
    public float WaitRefuseTime = 1.5f;
    public float WaitCrossRoadTime = 3f;
    private float _TimeToWait = -1f;
    private CrossRoadStateEnum _State;
    private CharacterController _CharacterController;

    public GameObject CrossRoadBound;

    #region  SpawnCar
    private bool _ShouldSpawnCar = true;
    public float CarSpeed = 3f;
    public float SpawnCarGapTime = 1.5f;
    private float _SpawnCarWaitTime = 3f;
    public float RandomSpan = 0.8f;
    public float CarDriveTime = 4f;
    public float CarPitchRange = 2f;
    public Transform SpawnIndicator;
    public Transform DestinationIndicator;
    public bool FirstCarPast = false;
    #endregion

    private bool Used = false;


    private void OnEnable()
    {
        FirstCarPast = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        var player = GameObject.Find("Player");
        _CharacterController = player.GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (_State is CrossRoadStateEnum.WaitForChangeShape && Input.GetButtonDown("Squat"))
        {
            _WaitOK();
        }
        else
        {

            if (_TimeToWait > 0)
            {
                _TimeToWait -= Time.deltaTime;

                if (_TimeToWait <= 0)
                {
                    _WaitOK();
                }
            }

            if (_SpawnCarWaitTime >= 0)
            {
                _SpawnCarWaitTime -= Time.deltaTime;

                if (_SpawnCarWaitTime <= 0)
                {
                    _SpawnCar();

                    _SpawnCarWaitTime = Random.Range(0f, RandomSpan) + SpawnCarGapTime;
                }
            }

            if (_State is CrossRoadStateEnum.Crossing)
            {
                Debug.Log("Mandatory  "+_TimeToWait.ToString());
                _CharacterController.MandatoryMove(new Vector2(3, 0));
            }

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name is "Player")
        {
            Activated();
        }
    }

    public void Activated()
    {
        if (Used)
        {
            return;
        }

        if (/*memory.played() == false*/false || _CharacterController.PlayerShape != PlayerShape.Ball || _CharacterController.IsSquating != false)
        {
            _Refuse();
        }
        else
        {
            _StartWaitingForChangeShape();
        }
    }

    private void _Refuse()
    {

        _CharacterController.ChangeMovable(false);
        _State = CrossRoadStateEnum.Refuse;
        _TimeToWait = WaitRefuseTime;
    }

    private void _StartWaitingForChangeShape()
    {
        Used = true;
        _CharacterController.ChangeMovable(false);
        _State = CrossRoadStateEnum.WaitForChangeShape;
    }

    private void _StartPlaying()
    {
        _CharacterController.MandatorySquat();
        _State = CrossRoadStateEnum.WaitNoCar;
        _TimeToWait = WaitNoCarTime;
        _ShouldSpawnCar = false;
    }

    private void _StartCrossing()
    {
        _State = CrossRoadStateEnum.Crossing;
        _TimeToWait = WaitCrossRoadTime;
    }

    private void _Finish()
    {
        _State = CrossRoadStateEnum.Finished;
        _CharacterController.ChangeMovable(true);
        _ShouldSpawnCar = true;
        Used = true;
        CrossRoadBound.GetComponent<MeshCollider>().enabled = true;
    }

    private void _WaitOK()
    {
        switch (_State)
        {
            case CrossRoadStateEnum.Refuse:
                _CharacterController.ChangeMovable(true);
                break;
            case CrossRoadStateEnum.WaitForChangeShape:
                _StartPlaying();
                break;
            case CrossRoadStateEnum.WaitNoCar:
                _StartCrossing();
                break;
            case CrossRoadStateEnum.Crossing:
                _Finish();
                break;
        }
    }

    void _SpawnCar()
    {
        if (_ShouldSpawnCar is false)
        {
            return;
        }

        GameObject obj = (GameObject)Resources.Load("Elements/Car");
        Instantiate(obj, GameObject.Find("OutSpace").transform);

        Car car = obj.GetComponent<Car>();
        car.Origination = SpawnIndicator.transform;
        car.Destination = DestinationIndicator.transform;
        car.DriveTime = CarDriveTime;
        car.PitchRange = CarPitchRange;
        car.IsFirstCar = !FirstCarPast;
        FirstCarPast = true;
    }
}
