using UnityEngine;
using UnityEngine.AI;

public class RunawayBehavior : StateMachineBehaviour
{
    private static readonly int IsInSafeHouseHash = Animator.StringToHash("IsInSafeHouse");

    private NavMeshAgent _agent;
    private FSMAIController _controller;
    private Transform _safeLocation;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("[AI] entrando en RunawayBehavior (fugindo da Safe Zone)");

        _controller = animator.GetComponent<FSMAIController>();
        _agent = _controller.Agent;
        _safeLocation = GameEnvironment.Singleton.safeLocation;
        _agent.isStopped = false;
        _agent.speed = 6;
        _agent.SetDestination(_safeLocation.position);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(_agent.remainingDistance < 1)
        {
            animator.SetBool(IsInSafeHouseHash, true);
        }
        else
        {
            animator.SetBool(IsInSafeHouseHash, false);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(IsInSafeHouseHash, false);
    }
}
