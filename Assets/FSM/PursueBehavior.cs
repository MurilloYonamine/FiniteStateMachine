using UnityEngine;
using UnityEngine.AI;

public class PursueBehavior : StateMachineBehaviour
{
    private static readonly int CanSeePlayerHash = Animator.StringToHash("CanSeePlayer");
    private static readonly int CanAttackPlayerHash = Animator.StringToHash("CanAttackPlayer");

    private NavMeshAgent _agent;
    private FSMAIController _controller;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("[AI] entrando en PursueBehavior");

        _controller = animator.GetComponent<FSMAIController>();
        _agent = _controller.Agent;
        _agent.speed = 5;
        _agent.isStopped = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent.SetDestination(_controller.Player.position);
        animator.SetBool(CanAttackPlayerHash, _controller.CanAttackPlayer());
        animator.SetBool(CanSeePlayerHash, _controller.CanSeePlayer());
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_agent.hasPath)
        {
            _agent.ResetPath();
        }
    }
}
