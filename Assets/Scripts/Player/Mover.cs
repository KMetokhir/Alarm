using System;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpingPower = 700f;
    [SerializeField] private float _gravityScale = 3;

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groungCheckerRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private float _horizontalMove;

    private Rigidbody2D _rigidbody;
    private bool _isInited = false;
    private bool _isMoving = false;

    public event Action<float> FacingChanged;
    public event Action<bool> GroundStatusChanged;
    public event Action StartsMoving;
    public event Action StopedMoving;

    private void Update()
    {
        if (_isInited == false) return;

        InvokeMovingEvents();
        GroundStatusChanged?.Invoke(!IsGrounded());
    }

    public void Init(Rigidbody2D rigidbody)
    {
        if (_isInited)
        {
            return;
        }

        _isInited = true;
        _rigidbody = rigidbody;

        _rigidbody.gravityScale = _gravityScale;
    }

    public void MoveHorizontal(float horizontalMove)
    {
        ThrowNotInitedException();

        if (horizontalMove != _horizontalMove)
        {
            FacingChanged?.Invoke(horizontalMove);
        }

        _horizontalMove = horizontalMove;
    }

    public void Jump()
    {
        ThrowNotInitedException();

        if (IsGrounded())
        {
            _rigidbody.AddForce(Vector2.up * jumpingPower);
        }
    }

    private void InvokeMovingEvents()
    {
        if (_rigidbody.velocity.x != 0 && _rigidbody.velocity.y == 0)
        {
            if (_isMoving == false)
            {
                _isMoving = true;
                StartsMoving?.Invoke();
            }
        }
        else if (_rigidbody.velocity.y == 0)
        {
            if (_isMoving == true)
            {
                _isMoving = false;
                StopedMoving?.Invoke();
            }
        }
    }

    private void Move()
    {
        if (IsGrounded())
        {
            _rigidbody.velocity = new Vector2(_horizontalMove * speed, _rigidbody.velocity.y);
        }
    }

    private void ThrowNotInitedException()
    {
        if (_isInited == false)
        {
            throw new Exception("Mover dosn't inited");
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(_groundCheck.position, _groungCheckerRadius, groundLayer);
    }
}