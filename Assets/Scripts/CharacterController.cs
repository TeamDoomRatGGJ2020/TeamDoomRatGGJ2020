using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum PlayerShape
{
    Normal,
    Ball,
    Square,
    Liquid
}

public class CharacterController : MonoBehaviour
{
    #region 属性设置
    public bool CanJump = false;
    // 弹跳力
    public float JumpForce = 400f;
    // 是否能在空中控制
    public bool CanAriControl = false;
    // 变成球的加速倍数
    public float BallSpeedRatio = 2f;
    #endregion


    // 地面所属Layer 判断是否滞空需要用
    public LayerMask GroundLayer;
    // 用于判断是否滞空的Object
    public Transform GroundCheck;
    // 检测是否滞空用的球面半径
    const float GroundCheckRadius = .1f;
    // 是否滞空
    private bool _IsGrounding;

    // 是否面朝右边
    private bool _FacingRight = true;

    private Vector3 _Velocity = Vector3.zero;

    #region 避免连续跳跃
    // 着地检测冷却时间
    const float GroudCheckColdTime = .1f;
    private float _NextGroundCheckTime = 0f;
    #endregion

    private Rigidbody _Rigidbody;

    public UnityEvent OnLandEvent;

    public PlayerShape PlayerShape = PlayerShape.Normal;

    private bool _IsKnocking = false;

    


    private void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody>();

        if (OnLandEvent is null)
        {
            OnLandEvent = new UnityEvent();

            OnLandEvent.AddListener(land);
        }
    }

    // 落地会发生什么
    private void land() { }

    private void FixedUpdate()
    {
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
                        OnLandEvent.Invoke();
                }
            }
        }

        //Debug.Log("IsGrounding = "+_IsGrounding.ToString());
    }

    public void Move(Vector2 move, bool jump)
    {
        if (PlayerShape == PlayerShape.Ball)
        {
            move *= BallSpeedRatio;
        }

        // 要么在地面 要么能够空中控制才能移动
        if (_IsGrounding || CanAriControl)
        {
            // 处理速度
            _Rigidbody.velocity = new Vector3(move.x, _Rigidbody.velocity.y, move.y);

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
        // 液体状态不转身？
        if (PlayerShape is PlayerShape.Liquid)
        {
            return;
        }

        _FacingRight = !_FacingRight;

        transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
    }

    public void SetPlayerShape(bool changeToBall, bool changeToSquare, bool changeToLiquid, bool isKnocking)
    {
        Debug.Log(PlayerShape.ToString() + " " + changeToBall.ToString() + " " + changeToSquare.ToString());
        switch (PlayerShape)
        {
            case PlayerShape.Normal:
                if (changeToBall)
                {
                    PlayerShape = PlayerShape.Ball;
                }
                else if (changeToSquare)
                {
                    PlayerShape = PlayerShape.Square;
                }
                else if (changeToLiquid)
                {
                    PlayerShape = PlayerShape.Liquid;
                }
                break;
            case PlayerShape.Ball:
                if (changeToBall)
                {
                    break;
                }
                else if (changeToSquare)
                {
                    PlayerShape = PlayerShape.Square;
                }
                else if (changeToLiquid)
                {
                    PlayerShape = PlayerShape.Liquid;
                }
                else
                {
                    PlayerShape = PlayerShape.Normal;
                }
                break;
            case PlayerShape.Square:
                if (changeToSquare)
                {
                    break;
                }
                else if (changeToBall)
                {
                    PlayerShape = PlayerShape.Ball;
                }
                else if (changeToLiquid)
                {
                    PlayerShape = PlayerShape.Liquid;
                }
                else
                {
                    PlayerShape = PlayerShape.Normal;
                }
                break;
            case PlayerShape.Liquid:
                if (changeToLiquid)
                {
                    break;
                }
                else if (changeToBall)
                {
                    PlayerShape = PlayerShape.Ball;
                }
                else if (changeToSquare)
                {
                    PlayerShape = PlayerShape.Square;
                }
                else
                {
                    PlayerShape = PlayerShape.Normal;
                }
                break;
        }

        if (PlayerShape is PlayerShape.Square)
        {
            _IsKnocking = true;
            knockEvent();
        }
    }

    private void knockEvent()
    {
        throw new NotImplementedException();
    }
}
