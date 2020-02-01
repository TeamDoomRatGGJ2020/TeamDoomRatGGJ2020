using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quarrel : MonoBehaviour
{
    private GameObject _Player;
    private GameObject _Shadow;
	private CharacterController _CharacterController;

    public Vector3 playerOffset = new Vector3(1.04f,0.76f,-1.34f);

    // Start is called before the first frame update
    void OnEnable()
    {
        _Player = GameObject.Find("Player");
        _CharacterController = _Player.GetComponent<CharacterController>();
        _Shadow = GameObject.Find("Shadow");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _Player.transform.position + playerOffset;

        _Player.SetActive(false);      
        _Shadow.SetActive(false);  
    }

    public void QuarrelFinish(){
        _Player.SetActive(true);
        _Shadow.SetActive(true);
        _CharacterController.ChangeMovable(true);

        Destroy(gameObject);
    }
}
