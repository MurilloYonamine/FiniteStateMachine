using UnityEngine;
using UnityEngine.AI;
public class Runaway : StateFSM
{
    Transform safeLocation;
    public Runaway(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base (_npc, _agent, _anim, _player) 
    {
        state = STATE.RUNAWAY;
        safeLocation = GameEnvironment.Singleton.safeLocation;
    }

    public override void Enter()
    {
        anim.SetTrigger("isRunning");
        agent.isStopped = false;
        agent.speed = 6.0f;
        agent.SetDestination(safeLocation.position);
        base.Enter();
    }

    public override void Update()
    {
        if(agent.remainingDistance < 1.0f)
        {
            nextState = new Idle(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isRunning");
        base.Exit();
    }
}
