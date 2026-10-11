// Author: Murillo Gomes Yonamine
// Date: 11/10/2026

using UnityEngine;

public class HideBehavior : StateMachineBehaviour
{
    private static readonly int RelocateHash = Animator.StringToHash("Relocate");
    private static readonly int FleeHash = Animator.StringToHash("Flee");
    private FSMAIController _npc;
    private Quaternion _baseRotation;
    private float _hideTime;
    [SerializeField] private float _relocateInterval = 30f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _npc = animator.GetComponent<FSMAIController>();
        _npc.Agent.isStopped = true;
        _npc.Agent.ResetPath();
        _baseRotation = animator.transform.rotation;
        _hideTime = 0f;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_npc.IsPaused) return;

        _hideTime += Time.deltaTime;
        animator.transform.rotation = _baseRotation * Quaternion.Euler(
            0f, Mathf.Sin(Time.time * _npc.ScanSpeed) * _npc.ScanAngle, 0f);

        if (_npc.EyesMeet() && _npc.FindNextHidePoint() != null)
            animator.SetTrigger(FleeHash);
        else if (_hideTime >= _relocateInterval && _npc.FindNextHidePoint() != null)
            animator.SetTrigger(RelocateHash);
    }
}
