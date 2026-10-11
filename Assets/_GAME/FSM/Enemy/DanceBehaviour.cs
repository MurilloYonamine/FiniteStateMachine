// Author: Murillo Gomes Yonamine
// Date: 11/10/2026

using UnityEngine;

public class DanceBehaviour : StateMachineBehaviour
{
    private FSMAIController _npc;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _npc = animator.GetComponent<FSMAIController>();
        StopNpc();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        StopNpc();
    }

    private void StopNpc()
    {
        if (!_npc.Agent.isActiveAndEnabled)
            return;

        _npc.Agent.isStopped = true;
        if (_npc.Agent.isOnNavMesh)
            _npc.Agent.ResetPath();
    }
}
