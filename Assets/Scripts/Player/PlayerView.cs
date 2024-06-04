using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private const string RunBoolVariable = "IsRuning";
    private const string JumpBoolVariable = "IsJumping";

    private bool _isFacingRight = true;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayRunAnimation()
    {
        _animator.SetBool(RunBoolVariable, true);
    }

    public void PlayIdleAnimation()
    {
        _animator.SetBool(RunBoolVariable, false);
    }

    public void SetJumpStatus(bool isJumping)
    {
        _animator.SetBool(JumpBoolVariable, isJumping);
    }

    public void Flip(float facingDirection)
    {
        if (_isFacingRight && facingDirection < 0f || !_isFacingRight && facingDirection > 0f)
        {
            _isFacingRight = !_isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}