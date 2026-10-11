// Author: Murillo Gomes Yonamine
// Date: 11/10/2026

using UnityEngine;

public class DeathBehavior : StateMachineBehaviour
{
    private Collider[] _colliders;
    private bool _finished;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FSMAIController controller = animator.GetComponent<FSMAIController>();
        controller.Agent.isStopped = true;
        controller.Agent.ResetPath();
        controller.Agent.enabled = false;
        controller.AudioSource.Stop();

        _colliders = animator.GetComponentsInChildren<Collider>();
        _finished = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_finished || stateInfo.normalizedTime < 1f) return;
        _finished = true;
        for (int i = 0; i < _colliders.Length; i++)
        {
            _colliders[i].enabled = false;
        }
        
        animator.GetComponent<FSMAIController>().FinishDeath();
    }
}
