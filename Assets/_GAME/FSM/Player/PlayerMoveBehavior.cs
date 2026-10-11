// Author: Murillo Gomes Yonamine
// Date: 04/10/2026

using UnityEngine;

public class PlayerMoveBehavior : StateMachineBehaviour
{
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private static readonly int JumpHash = Animator.StringToHash("Jump");

    private PlayerController _player;
    private Transform _transform;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player = animator.GetComponent<PlayerController>();
        _transform = animator.transform;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 input = _player.MoveInput;

        if (input == Vector2.zero)
        {
            animator.SetBool(IsMovingHash, false);
            return;
        }

        bool isRunning = _player.SprintHeld;
        float speedParam = input.y < 0 ? -1f : (isRunning ? 1f : 0.5f);
        animator.SetFloat(SpeedHash, speedParam);

        Vector3 moveDir = (_transform.forward * input.y + _transform.right * input.x).normalized;
        float currentSpeed = isRunning ? _player.RunSpeed : _player.WalkSpeed;
        _player.Move(moveDir, currentSpeed);

        if (_player.ConsumeJump() && _player.IsGrounded)
        {
            _player.Jump();
            animator.SetTrigger(JumpHash);
        }
    }
}
