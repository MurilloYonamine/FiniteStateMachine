// Author: Murillo Gomes Yonamine
// Date: 11/10/2026

using UnityEngine;

public class FleeBehavior : StateMachineBehaviour
{
    private static readonly int ArrivedHash = Animator.StringToHash("Arrived");
    [SerializeField] private float _speed = 6f;
    private FSMAIController _npc;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _npc = animator.GetComponent<FSMAIController>();
        if (!_npc.GoTo(_npc.FindNextHidePoint(), _speed))
            animator.SetTrigger(ArrivedHash);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_npc.Agent.isActiveAndEnabled || !_npc.Agent.isOnNavMesh)
            return;

        if (!_npc.Agent.pathPending && _npc.Agent.remainingDistance < 1f)
            animator.SetTrigger(ArrivedHash);
    }
}
