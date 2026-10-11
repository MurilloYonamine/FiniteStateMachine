// Author: Murillo Gomes Yonamine
// Date: 04/10/2026

using UnityEngine;

public class PlayerIdleBehavior : StateMachineBehaviour
{
    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private static readonly int JumpHash = Animator.StringToHash("Jump");

    private PlayerController _player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player = animator.GetComponent<PlayerController>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_player.MoveInput != Vector2.zero)
            animator.SetBool(IsMovingHash, true);

        if (_player.ConsumeJump() && _player.IsGrounded)
        {
            _player.Jump();
            animator.SetTrigger(JumpHash);
        }
    }
}
