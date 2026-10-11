// Author: Murillo Gomes Yonamine
// Date: 04/10/2026

using UnityEngine;

public class PlayerJumpBehavior : StateMachineBehaviour
{
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");

    private PlayerController _player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player = animator.GetComponent<PlayerController>();
        animator.SetBool(IsGroundedHash, false);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(IsGroundedHash, _player.IsGrounded);
    }
}
