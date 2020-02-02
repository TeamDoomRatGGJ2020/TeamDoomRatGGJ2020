using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairBridgeController : MonoBehaviour
{
    public GameObject bound;

    public void Boom()
    {
        bound.transform.localScale = new Vector3(1, 1, 1);
    }
}
