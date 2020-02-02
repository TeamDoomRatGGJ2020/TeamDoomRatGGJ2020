using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    // public Transform Origination{set{_Origination = value;}}
    public Transform Origination;
    // public Transform Destination{set{_Destination = value;}}
    public Transform Destination;
    public float DriveTime = 6f;
    private float _PastTime = 0f;
    public float PitchRange;
    public string CarSpriteUrlBase = "Elements/Car";

    private SpriteRenderer _SpriteRenderer;

    private AudioSource _AudioSource;

    public bool IsFirstCar = false;

    // Start is called before the first frame update
    void Start()
    {
        _AudioSource = GetComponent<AudioSource>();
        _AudioSource.pitch = _AudioSource.pitch + Random.Range(-0.5f,0.5f)*PitchRange;
        

        string carUrl = CarSpriteUrlBase;
        switch(Random.Range(0,3)){
            case 0:
                carUrl+="1";
                break;
            case 1:
                carUrl +="2";
                break;
            case 2:
                carUrl += "3";
                break;
            default:
                carUrl += "3";
                break;
        }
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        _SpriteRenderer.sprite = Resources.Load<Sprite>(carUrl);
    }

    // Update is called once per frame
    void Update()
    {
        if(_AudioSource.isPlaying is false){
            GameFacade.Instance.PlayNormalSound(AudioManager.Sound_Car,_AudioSource);
        }

        _PastTime += Time.deltaTime;

        if(_PastTime > DriveTime){

            Destroy(gameObject);
        }

        transform.position = Vector3.Lerp(Origination.position,Destination.position,_PastTime/DriveTime);
        transform.localScale = Vector3.Lerp(Origination.localScale,Destination.localScale,_PastTime/DriveTime);

        if (IsFirstCar)
        {
            _PastTime = 0f;
            gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        }
    }


}
