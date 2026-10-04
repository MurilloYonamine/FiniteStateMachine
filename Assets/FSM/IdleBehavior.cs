using UnityEngine;
using UnityEngine.AI;

public class IdleBehavior : StateMachineBehaviour
{
    private static readonly int CanSeePlayerHash = Animator.StringToHash("CanSeePlayer");
    private static readonly int IdleToPatrolHash = Animator.StringToHash("IdleToPatrol");
    
    private NavMeshAgent _agent;
    private FSMAIController _controller;
    [SerializeField] private float _transitionDelay = 3f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("[AI] entrando en IdleBehavior");

        _controller = animator.GetComponent<FSMAIController>();
        _agent = _controller.Agent;
        _agent.isStopped = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(CanSeePlayerHash, _controller.CanSeePlayer());
        if (_transitionDelay > 0)
        {
            _transitionDelay -= Time.deltaTime;
            return;
        }
        
        animator.SetBool(IdleToPatrolHash, true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(IdleToPatrolHash, false);
        _transitionDelay = 3f;
    }
}
