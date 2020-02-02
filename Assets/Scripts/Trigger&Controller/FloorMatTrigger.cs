using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorMatTrigger : MonoBehaviour
{
    public FloorMaterial material;
    private Collider _PlayerCollider;
	private CharacterController _CharacterController;

    private void Awake()
	{
        var player = GameObject.Find("Player");
        _PlayerCollider = player.GetComponent<Collider>();
		_CharacterController = player.GetComponent<CharacterController>();
	}

    void OnTriggerEnter(Collider other)
	{
        if(other == _PlayerCollider)
        {
            _CharacterController.ChangeFloorMaterial(material);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other == _PlayerCollider)
        {
            _CharacterController.ChangeFloorMaterial(FloorMaterial.Grass);
        }
    }

}
