using UnityEngine;
using UnityEngine.AI;

public class AttackBehavior : StateMachineBehaviour
{
    private static readonly int CanAttackPlayerHash = Animator.StringToHash("CanAttackPlayer");

    private NavMeshAgent _agent;
    private FSMAIController _controller;
    private AudioSource _shootAudioSource;
    private float _rotationSpeed = 2f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("[AI] entrando en AttackBehavior");

        _controller = animator.GetComponent<FSMAIController>();
        _agent = _controller.Agent;
        _shootAudioSource = _controller.AudioSource;
        _agent.velocity = Vector3.zero;
        _agent.isStopped = true;
        _shootAudioSource.Play();
        if (!_shootAudioSource.isPlaying)
            _shootAudioSource.Play();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 direction = _controller.Player.position - _controller.gameObject.transform.position;
        direction.y = 0;
        _controller.gameObject.transform.rotation = Quaternion.Slerp(
            a: _controller.gameObject.transform.rotation, 
            b: Quaternion.LookRotation(direction), 
            t: _rotationSpeed * Time.deltaTime
        );
        animator.SetBool(CanAttackPlayerHash, _controller.CanAttackPlayer());
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _shootAudioSource.Stop();
    }
}
