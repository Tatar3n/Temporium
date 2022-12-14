using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private RaycastHit2D _checkGroundRay;
    private RaycastHit2D _checkBorderHeadRay;
    private RaycastHit2D _checkBorderBodyRay;
    private Vector3 _leftFlip = new Vector3(0, 180, 0);
    private Vector2 _horizontalVelocity;
    private float _horizontalSpeed;
    private float _verticalSpeed;
    private float _signPreviosFrame;
    private float _signCurrentFrame;
    private bool _isGround;
    private bool _isBorder;


    public Transform CheckBorderHeadRayTransform;
    public Transform CheckBorderBodyRayTransform;
    public LayerMask BorderLayerMask;
    public LayerMask GroundLayerMask;
    public float MoveSpeed;
    public float JumpForce;
    public float RayDistance;



    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        _horizontalSpeed = Input.GetAxis("Horizontal");
        _verticalSpeed = Input.GetAxis("Vertical");

        StateUpdate();
        Jump();
        Flip();
    }

    private void StateUpdate()
    {
        _checkGroundRay = Physics2D.Raycast
            (
                transform.position,
                -Vector2.up,
                RayDistance,
                GroundLayerMask
            );
        _isGround = _checkGroundRay;

        _checkBorderHeadRay = Physics2D.Raycast
            (
                CheckBorderHeadRayTransform.position,
                CheckBorderHeadRayTransform.right,
                RayDistance,
                BorderLayerMask
            );
        _checkBorderBodyRay = Physics2D.Raycast
            (
                CheckBorderBodyRayTransform.position,
                CheckBorderBodyRayTransform.right,
                RayDistance,
                BorderLayerMask
            );
        _isBorder = _checkBorderHeadRay || _checkBorderBodyRay;

        Debug.DrawRay
            (
                transform.position,
                -Vector2.up * RayDistance,
                Color.red
            );
        Debug.DrawRay
           (
               CheckBorderHeadRayTransform.position,
                CheckBorderHeadRayTransform.right * RayDistance,
               Color.red
           );
        Debug.DrawRay
           (
               CheckBorderBodyRayTransform.position,
                CheckBorderBodyRayTransform.right * RayDistance,
               Color.red
           );
    }

    private void Move()
    {
        if (!_isBorder)
        {
            _horizontalVelocity.Set(_horizontalSpeed * MoveSpeed, _rigidbody.velocity.y);
            _rigidbody.velocity = _horizontalVelocity;
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && _isGround)
            _rigidbody.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
    }

    private void Flip()
    {
        _signCurrentFrame = _horizontalSpeed == 0 ? _signPreviosFrame : Mathf.Sign(_horizontalSpeed);

        if (_signCurrentFrame != _signPreviosFrame)
        {
            transform.rotation = Quaternion.Euler(_horizontalSpeed < 0 ? _leftFlip : Vector3.zero);
        }
        _signPreviosFrame = _signCurrentFrame;
    }

}
