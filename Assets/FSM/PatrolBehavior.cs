using UnityEngine;
using UnityEngine.AI;

public class PatrolBehavior : StateMachineBehaviour
{
    private static readonly int CanSeePlayerHash = Animator.StringToHash("CanSeePlayer");
    private static readonly int IsPlayerBehindHash = Animator.StringToHash("IsPlayerBehind");

    private NavMeshAgent _agent;
    private FSMAIController _controller;
    private int _currentIndex = -1;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("[AI] entrando en PatrolBehavior");

        _controller = animator.GetComponent<FSMAIController>();
        _agent = _controller.Agent;
        _agent.speed = 2;
        _agent.isStopped = false;

        float lastDistance = Mathf.Infinity;

        for (int i = 0; i < GameEnvironment.Singleton.Checkpoints.Count; i++)
        {
            float distance = Vector3.Distance(_agent.transform.position, GameEnvironment.Singleton.Checkpoints[i].transform.position);
            if (distance < lastDistance)
            {
                _currentIndex = i - 1;
                lastDistance = distance;
            }
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_agent.remainingDistance < 1)
        {
            _currentIndex =
                _currentIndex >= GameEnvironment.Singleton.Checkpoints.Count - 1 ? 
                0 : 
                _currentIndex + 1;
            
            _agent.SetDestination(GameEnvironment.Singleton.Checkpoints[_currentIndex].transform.position);
        }

        animator.SetBool(CanSeePlayerHash, _controller.CanSeePlayer());
        animator.SetBool(IsPlayerBehindHash, _controller.IsPlayerBehind());
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(IsPlayerBehindHash, false);
    }
}
