using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController : MonoBehaviour
{
    #region 属性设置
    // 弹跳力
    public float JumpForce = 400f;
    // 是否能在空中控制
    public bool CanAriControl = false;
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

        if (_IsGrounding && jump)
        {
            _IsGrounding = false;

            _Rigidbody.AddForce(new Vector3(0f, JumpForce, 0f));
            _NextGroundCheckTime = Time.time + GroudCheckColdTime;
        }
    }

    private void Flip()
    {
        _FacingRight = !_FacingRight;

        transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
    }
}
