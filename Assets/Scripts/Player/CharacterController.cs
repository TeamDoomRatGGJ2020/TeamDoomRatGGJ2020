using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum PlayerShape
{
    Ball,
    Square
}

public enum FloorMaterial
{
    Grass,
    Plank,
    FootPath,
    Road
}

public struct ShapeButtonCondition
{
    public bool BallButtonDown;
    public bool SquareButtonDown;
    public bool SquatingButtonDown;
}

public class CharacterController : MonoBehaviour
{
    #region 属性设置
    public bool CanJump = false;
    // 弹跳力
    public float JumpForce = 400f;
    // 是否能在空中控制
    public bool CanAriControl = false;
    // 缩成球的加速倍数
    public float BallSpeedRatio = 2f;
    // 变成方形的加速倍数
    public float SquareSpeedRatio = 0.75f;
    public float NormalColliderRadius = 5f;
    public float SquatingColliderRadius = 2.5f;
    public float WalkThres = .1f;
    #endregion

    #region 滞空相关 没有跳跃那就没用
    // 地面所属Layer 判断是否滞空需要用
    public LayerMask GroundLayer;
    // 用于判断是否滞空的Object
    public Transform GroundCheck;
    // 检测是否滞空用的球面半径
    const float GroundCheckRadius = .1f;
    // 是否滞空
    private bool _IsGrounding;
    #endregion

    // 是否面朝右边
    private bool _FacingRight;
    private Vector2 _Move;
    private bool _Jump;
    public Vector2 Speed = new Vector2(1, 1);
    //private Vector3 _Velocity = Vector3.zero;

    #region 避免连续跳跃
    // 着地检测冷却时间
    const float GroudCheckColdTime = .1f;
    private float _NextGroundCheckTime = 0f;
    #endregion

    private Rigidbody _Rigidbody;
    private UnityEvent _OnLandEvent;
    private Animator _Animator;
    private SphereCollider _SphereCollider;
    private Animator _EyesAnimator;
    private AudioSource _AudioSource;

    public PlayerShape PlayerShape = PlayerShape.Ball;
    // 保证每次检测伸缩脚之前该键已经被抬起来了 避免按住然后连续伸缩
    private bool _SquatingKeyReset = true;
    private bool _IsSquating = false;
    private bool _ChangeToBall = false;
    private bool _ChangeToSquare = false;

    private FloorMaterial _FloorMaterial = FloorMaterial.Grass;

    private void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody>();
        _Animator = GetComponent<Animator>();
        _SphereCollider = GetComponent<SphereCollider>();
        _EyesAnimator = GameObject.Find("Eyes").GetComponent<Animator>();
        _AudioSource = GetComponent<AudioSource>();

        if (_OnLandEvent is null)
        {
            _OnLandEvent = new UnityEvent();

            _OnLandEvent.AddListener(land);
        }
    }

    // 落地会发生什么 震动苹果树？
    private void land()
    {
        // TODO
    }

    private ShapeButtonCondition _UpdateInput()
    {
        // Movement
        _Move.x = Input.GetAxis("Horizontal");
        _Move.y = Input.GetAxis("Vertical");
        _Move *= Speed;

        _Jump = Input.GetButton("Jump");


        // ShapeChange
        bool ballButtonDown = Input.GetButton("BallMode");
        bool squareButtonDown = Input.GetButton("SquareMode");
        bool squatingButtonDown = Input.GetButton("Squat");
        if (squatingButtonDown is false)
        {
            _SquatingKeyReset = true;
        }
        return new ShapeButtonCondition
        {
            BallButtonDown = ballButtonDown,
            SquareButtonDown = squareButtonDown,
            SquatingButtonDown = squatingButtonDown
        };
    }

    private void _UpdateShape(ShapeButtonCondition shapeButtonCondition)
    {
        SetPlayerShape(shapeButtonCondition.BallButtonDown, shapeButtonCondition.SquareButtonDown, shapeButtonCondition.SquatingButtonDown);
    }

    private void _UpdateAnimation()
    {
        float velocity = _Rigidbody.velocity.magnitude;
        _Animator.SetFloat("Speed", velocity);
        //_Animator.SetBool("IsBall", PlayerShape is PlayerShape.Ball);
        //_Animator.SetBool("IsSquare", PlayerShape is PlayerShape.Square);
        _Animator.SetBool("ChangeBallTrigger", _ChangeToBall);
        _Animator.SetBool("ChangeSquareTrigger", _ChangeToSquare);
        _ChangeToBall = false;
        _ChangeToSquare = false;

        _Animator.SetBool("IsSquating", _IsSquating);
    }

    private void Update()
    {
        var buttonCondition = _UpdateInput();

        _UpdateShape(buttonCondition);

        _UpdateAnimation();

        _UpdateWalkAudio();
    }

    public void ChangeFloorMaterial(FloorMaterial floorMaterial)
    {
        GameFacade.Instance.StopNormalSound(_AudioSource);

        _FloorMaterial = floorMaterial;
    }

    private void _UpdateWalkAudio()
    {
        float velocity = new Vector3(_Rigidbody.velocity.x, 0, _Rigidbody.velocity.z).magnitude;
        if (velocity > WalkThres)
        {
            if (_AudioSource.isPlaying)
            {
                return;
            }

            String SoundName = "";

            switch (_FloorMaterial)
            {
                case FloorMaterial.Grass:
                    SoundName = AudioManager.Sound_Walk_Grass;
                    break;
                case FloorMaterial.Plank:
                    SoundName = AudioManager.Sound_Walk_Plank;
                    break;
                case FloorMaterial.FootPath:
                    SoundName = AudioManager.Sound_Walk_Foodpath;
                    break;
                case FloorMaterial.Road:
                    SoundName = AudioManager.Sound_Walk_Road;
                    break;
            }

            GameFacade.Instance.PlayNormalSound(SoundName, _AudioSource);
        }
        else
        {
            if (_AudioSource.clip.name.Contains("走路"))
            {
                GameFacade.Instance.StopNormalSound(_AudioSource);
            }
        }
    }

    private void FixedUpdate()
    {
        Move(_Move, _Jump);

        bool wasGrounding = _IsGrounding;
        _IsGrounding = false;

        // 检测是否和地面碰撞
        if (Time.time > _NextGroundCheckTime)
        {
            Collider[] colliders = Physics.OverlapSphere(GroundCheck.position, GroundCheckRadius, GroundLayer);
            foreach (var i in colliders)
            {
                if (i.gameObject != gameObject)
                {
                    _IsGrounding = true;
                    if (!wasGrounding)
                        _OnLandEvent.Invoke();
                }
            }
        }
    }

    public void Move(Vector2 move, bool jump)
    {
        if (PlayerShape == PlayerShape.Ball)
        {
            if (_IsSquating)
            {
                move *= BallSpeedRatio;
            }
        }
        else
        {
            if (_IsSquating)
            {
                move = Vector2.zero;
            }
            else
            {
                move *= SquareSpeedRatio;
            }
        }

        // 要么在地面 要么能够空中控制才能移动
        if (_IsGrounding || CanAriControl)
        {
            // 处理速度
            _Rigidbody.velocity = new Vector3(move.x, _Rigidbody.velocity.y, move.y);
            _FacingRight = transform.localScale.x > 0;

            // 处理左右朝向反面
            if (move.x > 0 && !_FacingRight)
            {
                Flip();
            }
            else if (move.x < 0 && _FacingRight)
            {
                Flip();
            }
        }

        if (CanJump && _IsGrounding && jump)
        {
            _IsGrounding = false;

            _Rigidbody.AddForce(new Vector3(0f, JumpForce, 0f));
            _NextGroundCheckTime = Time.time + GroudCheckColdTime;
        }
    }

    private void Flip()
    {
        // _FacingRight = !_FacingRight;

        transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
    }

    public void SetPlayerShape(bool changeToBall, bool changeToSquare, bool changeSquat)
    {
        //Debug.Log(_SquatingKeyReset.ToString()+" "+PlayerShape.ToString()+" "+changeToBall.ToString()+" "+changeToSquare.ToString());

        changeSquat &= _SquatingKeyReset;

        switch (PlayerShape)
        {
            case PlayerShape.Ball:
                if (changeToBall is false && changeToSquare is true)
                {
                    PlayerShape = PlayerShape.Square;
                    _ChangeToSquare = true;
                    changeSquat = (_IsSquating);
                }
                break;
            case PlayerShape.Square:
                if (changeToSquare is false && changeToBall is true)
                {
                    PlayerShape = PlayerShape.Ball;
                    _ChangeToBall = true;
                    changeSquat = (_IsSquating);
                }
                break;
        }

        // 处理缩脚
        if (changeSquat)
        {
            changeSquating();
        }
    }

    private void changeSquating()
    {
        _SquatingKeyReset = false;
        _IsSquating = !_IsSquating;

        if (_IsSquating)
        {
            _SphereCollider.radius = SquatingColliderRadius;

            if (PlayerShape is PlayerShape.Square)
            {
                knockEvent();
            }
        }
        else
        {
            _SphereCollider.radius = NormalColliderRadius;
        }
    }

    private void knockEvent()
    {
        if (PlayerShape is PlayerShape.Square)
        {
            GameFacade.Instance.PlayNormalSound(AudioManager.Sound_Smash, _AudioSource);
        }
    }

    public void ShakeHead()
    {
        _EyesAnimator.SetTrigger("ShakeHeadTrigger");
    }

    public void EatApple()
    {
        GameFacade.Instance.PlayNormalSound(AudioManager.Sound_EatApple, _AudioSource);
        // TODO
    }


}
