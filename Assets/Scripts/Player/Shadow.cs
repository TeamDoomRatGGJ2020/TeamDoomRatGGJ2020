using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
	public Transform Player;


	// Update is called once per frame
	void Update()
	{
		gameObject.transform.position = new Vector3(Player.position.x,0,Player.position.z);
	}
}
