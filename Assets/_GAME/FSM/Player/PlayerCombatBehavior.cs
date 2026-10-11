// Author: Murillo Gomes Yonamine
// Date: 04/10/2026

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombatBehavior : StateMachineBehaviour
{
    private static readonly int FireHash = Animator.StringToHash("Fire");
    private static readonly int ReloadHash = Animator.StringToHash("Reload");

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var mouse = Mouse.current;
        var keyboard = Keyboard.current;

        if (mouse != null && mouse.leftButton.wasPressedThisFrame)
            animator.SetTrigger(FireHash);

        if (keyboard != null && keyboard.rKey.wasPressedThisFrame)
            animator.SetTrigger(ReloadHash);
    }
}
