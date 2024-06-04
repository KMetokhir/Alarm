using UnityEngine;

[RequireComponent(typeof(Mover), typeof(Rigidbody2D), typeof(PlayerView))]
public class Player : MonoBehaviour
{
    private const string JumpButton = "Jump";
    private const string HorizontalAxis = "Horizontal";

    private Mover _mover;
    private PlayerView _view;   

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        _view = GetComponent<PlayerView>();

        _mover.Init(rigidbody);
    }

    private void OnEnable()
    {
        _mover.FacingChanged += OnFacingchanged;
        _mover.StartsMoving += OnStartsMoving;
        _mover.StopedMoving += OnStopedMoving;
        _mover.GroundStatusChanged += OnGroundStatusChanged;
    }

    private void OnDisable()
    {
        _mover.FacingChanged -= OnFacingchanged;
        _mover.StartsMoving -= OnStartsMoving;
        _mover.StopedMoving -= OnStopedMoving;
        _mover.GroundStatusChanged -= OnGroundStatusChanged;

    }

    private void Update()
    {
        float horizontalMove = Input.GetAxisRaw(HorizontalAxis);
        _mover.MoveHorizontal(horizontalMove);

        if (Input.GetButtonDown(JumpButton))
        {
            _mover.Jump();
        }
    }

    private void OnGroundStatusChanged(bool isGrounded)
    {
        _view.SetJumpStatus(isGrounded);
    }

    private void OnStopedMoving()
    {
        _view.PlayIdleAnimation();
    }

    private void OnStartsMoving()
    {
        _view.PlayRunAnimation();
    }

    private void OnFacingchanged(float facingDirection)
    {
        _view.Flip(facingDirection);
    }
}