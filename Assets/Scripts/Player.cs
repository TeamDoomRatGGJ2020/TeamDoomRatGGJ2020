using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _CharacterController;

    private Vector2 _Move;
    private bool _Jump;
    public Vector2 Speed = new Vector2(1, 1);

    // Start is called before the first frame update
    void Start()
    {
        _CharacterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        _Move.x = Input.GetAxis("Horizontal");
        _Move.y = Input.GetAxis("Vertical");
        _Move *= Speed;

        _Jump = Input.GetButton("Jump");

        bool ballButtonDown = Input.GetButton("BallMode");
        bool squareButtonDown = Input.GetButton("SquareMode");
        bool liquidButtonDown = Input.GetButton("LiquidMode");
        bool knockButtonDown = Input.GetButton("Knock");
        _CharacterController.SetPlayerShape(ballButtonDown, squareButtonDown, liquidButtonDown,knockButtonDown);
    }

    private void FixedUpdate()
    {
        _CharacterController.Move(_Move, _Jump);
    }
}
